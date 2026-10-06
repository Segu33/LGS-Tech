using System.Security.Claims;
using LGS.Tech.Models;
using LGS.Tech.Repositories;
using LGS.Tech.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LGS.Tech.Controllers
{
    [Authorize(Roles = "Administrador,Tecnico")]
    public class ArchivosOrdenController : Controller
    {
        private readonly IArchivoOrdenRepository _archivoRepository;
        private readonly IOrdenReparacionRepository _ordenRepository;
        private readonly IWebHostEnvironment _environment;

        public ArchivosOrdenController(
            IArchivoOrdenRepository archivoRepository,
            IOrdenReparacionRepository ordenRepository,
            IWebHostEnvironment environment)
        {
            _archivoRepository = archivoRepository;
            _ordenRepository = ordenRepository;
            _environment = environment;
        }

        // ==========================================
        // SUBIR - GET
        // Solo Administrador
        // ==========================================

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Subir(int ordenId)
        {
            var orden = await _ordenRepository
                .ObtenerPorIdAsync(ordenId);

            if (orden == null)
            {
                return NotFound();
            }

            var viewModel = new ArchivoOrdenFormViewModel
            {
                OrdenReparacionId = orden.Id
            };

            ViewBag.Orden = orden;

            return View(viewModel);
        }

        // ==========================================
        // SUBIR - POST
        // Solo Administrador
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Subir(
            ArchivoOrdenFormViewModel viewModel)
        {
            var orden = await _ordenRepository
                .ObtenerPorIdAsync(viewModel.OrdenReparacionId);

            if (orden == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Orden = orden;

                return View(viewModel);
            }

            if (viewModel.Archivo == null ||
                viewModel.Archivo.Length == 0)
            {
                ModelState.AddModelError(
                    nameof(viewModel.Archivo),
                    "Debe seleccionar un archivo.");

                ViewBag.Orden = orden;

                return View(viewModel);
            }

            // ==========================================
            // VALIDAR TAMAÑO
            // Máximo 10 MB
            // ==========================================

            const long tamañoMaximo = 10 * 1024 * 1024;

            if (viewModel.Archivo.Length > tamañoMaximo)
            {
                ModelState.AddModelError(
                    nameof(viewModel.Archivo),
                    "El archivo no puede superar los 10 MB.");

                ViewBag.Orden = orden;

                return View(viewModel);
            }

            // ==========================================
            // VALIDAR EXTENSIÓN
            // ==========================================

            var extensionesPermitidas = new[]
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".pdf"
            };

            var extension = Path.GetExtension(
                viewModel.Archivo.FileName
            ).ToLowerInvariant();

            if (!extensionesPermitidas.Contains(extension))
            {
                ModelState.AddModelError(
                    nameof(viewModel.Archivo),
                    "Solo se permiten archivos JPG, JPEG, PNG o PDF.");

                ViewBag.Orden = orden;

                return View(viewModel);
            }

            // ==========================================
            // DEFINIR TIPO DE ARCHIVO
            // No confiamos en ContentType del navegador
            // ==========================================

            var tipoArchivo = extension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".pdf" => "application/pdf",
                _ => "application/octet-stream"
            };

            // ==========================================
            // NOMBRE ORIGINAL
            // ==========================================

            var nombreOriginal = Path.GetFileName(
                viewModel.Archivo.FileName
            );

            if (nombreOriginal.Length > 255)
            {
                ModelState.AddModelError(
                    nameof(viewModel.Archivo),
                    "El nombre del archivo es demasiado largo.");

                ViewBag.Orden = orden;

                return View(viewModel);
            }

            // ==========================================
            // CARPETA PRIVADA
            //
            // LGS.Tech/
            // └── PrivateFiles/
            //     └── ordenes/
            //         └── 1/
            // ==========================================

            var carpeta = Path.Combine(
                _environment.ContentRootPath,
                "PrivateFiles",
                "ordenes",
                orden.Id.ToString()
            );

            Directory.CreateDirectory(carpeta);

            // Nombre interno único para evitar
            // archivos con nombres repetidos.
            var nombreInterno =
                $"{Guid.NewGuid()}{extension}";

            var rutaFisica = Path.Combine(
                carpeta,
                nombreInterno
            );

            // ==========================================
            // GUARDAR ARCHIVO FÍSICO
            // ==========================================

            using (var stream = new FileStream(
                rutaFisica,
                FileMode.Create))
            {
                await viewModel.Archivo.CopyToAsync(stream);
            }

            // Guardamos una ruta relativa en MariaDB.
            // Usamos "/" para que no dependa de Windows.
            var rutaRelativa =
                $"ordenes/{orden.Id}/{nombreInterno}";

            var archivo = new ArchivoOrden
            {
                OrdenReparacionId = orden.Id,

                NombreArchivo = nombreOriginal,

                RutaArchivo = rutaRelativa,

                TipoArchivo = tipoArchivo,

                FechaSubida = DateTime.Now
            };

            try
            {
                await _archivoRepository.AgregarAsync(archivo);
            }
            catch
            {
                // Si fallara MariaDB después de guardar
                // el archivo, evitamos dejarlo abandonado.
                if (System.IO.File.Exists(rutaFisica))
                {
                    System.IO.File.Delete(rutaFisica);
                }

                throw;
            }

            TempData["Mensaje"] =
                "Archivo subido correctamente.";

            return RedirectToAction(
                "Detalle",
                "OrdenesReparacion",
                new { id = orden.Id });
        }

        // ==========================================
        // ABRIR ARCHIVO
        //
        // Administrador:
        // puede abrir cualquier archivo.
        //
        // Técnico:
        // solamente archivos de órdenes
        // asignadas a él.
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Abrir(int id)
        {
            var archivo = await _archivoRepository
                .ObtenerPorIdAsync(id);

            if (archivo == null)
            {
                return NotFound();
            }

            var orden = await _ordenRepository
                .ObtenerPorIdAsync(
                    archivo.OrdenReparacionId
                );

            if (orden == null)
            {
                return NotFound();
            }

            // Si es Técnico, verificamos que
            // la orden esté asignada a ese usuario.
            if (User.IsInRole("Tecnico") &&
                !User.IsInRole("Administrador"))
            {
                var tecnicoId = User.FindFirstValue(
                    ClaimTypes.NameIdentifier
                );

                if (orden.TecnicoId != tecnicoId)
                {
                    return Forbid();
                }
            }

            var rutaFisica = ObtenerRutaFisica(
                archivo.RutaArchivo
            );

            if (!System.IO.File.Exists(rutaFisica))
            {
                return NotFound();
            }

            return PhysicalFile(
                rutaFisica,
                archivo.TipoArchivo
            );
        }

        // ==========================================
        // ELIMINAR
        // Solo Administrador
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var archivo = await _archivoRepository
                .ObtenerPorIdAsync(id);

            if (archivo == null)
            {
                return NotFound();
            }

            var ordenId =
                archivo.OrdenReparacionId;

            var rutaFisica = ObtenerRutaFisica(
                archivo.RutaArchivo
            );

            if (System.IO.File.Exists(rutaFisica))
            {
                System.IO.File.Delete(rutaFisica);
            }

            await _archivoRepository
                .EliminarAsync(archivo);

            TempData["Mensaje"] =
                "Archivo eliminado correctamente.";

            return RedirectToAction(
                "Detalle",
                "OrdenesReparacion",
                new { id = ordenId });
        }

        // ==========================================
        // MÉTODO AUXILIAR
        // Convierte la ruta guardada en MariaDB
        // en la ruta física privada del servidor.
        // ==========================================

        private string ObtenerRutaFisica(
            string rutaRelativa)
        {
            var rutaNormalizada =
                rutaRelativa.Replace(
                    '/',
                    Path.DirectorySeparatorChar
                );

            return Path.Combine(
                _environment.ContentRootPath,
                "PrivateFiles",
                rutaNormalizada
            );
        }
    }
}