using System.Security.Claims;
using LGS.Tech.Models;
using LGS.Tech.Repositories;
using LGS.Tech.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LGS.Tech.Controllers
{
    [Authorize(Roles = "Administrador,Tecnico")]
    public class OrdenesReparacionController : Controller
    {
        private readonly IOrdenReparacionRepository _ordenRepository;
        private readonly IEquipoRepository _equipoRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPagoRepository _pagoRepository;
        private readonly IArchivoOrdenRepository _archivoRepository;

        public OrdenesReparacionController(
            IOrdenReparacionRepository ordenRepository,
            IEquipoRepository equipoRepository,
            UserManager<ApplicationUser> userManager,
            IPagoRepository pagoRepository,
            IArchivoOrdenRepository archivoRepository)
        {
            _ordenRepository = ordenRepository;
            _equipoRepository = equipoRepository;
            _userManager = userManager;
            _pagoRepository = pagoRepository;
            _archivoRepository = archivoRepository;
        }

        // ==========================================
        // LISTADO
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Index(
            string? buscar = null,
            EstadoOrden? estado = null,
            int pagina = 1,
            int cantidadPorPagina = 10)
        {
            if (pagina < 1)
                pagina = 1;

            if (cantidadPorPagina < 1)
                cantidadPorPagina = 10;

            string? tecnicoId = null;

            if (User.IsInRole("Tecnico") &&
                !User.IsInRole("Administrador"))
            {
                tecnicoId = User.FindFirstValue(
                    ClaimTypes.NameIdentifier);
            }

            var ordenes = await _ordenRepository.BuscarAsync(
                buscar,
                estado,
                pagina,
                cantidadPorPagina,
                tecnicoId);

            var totalRegistros =
                await _ordenRepository.ContarAsync(
                    buscar,
                    estado,
                    tecnicoId);

            var totalPaginas = (int)Math.Ceiling(
                totalRegistros / (double)cantidadPorPagina);

            ViewBag.Buscar = buscar;
            ViewBag.Estado = estado;
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;
            ViewBag.TotalRegistros = totalRegistros;

            return View(ordenes);
        }
        // ==========================================
        // DETALLE
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var orden = await _ordenRepository.ObtenerPorIdAsync(id);

            if (orden == null)
            {
                return NotFound();
            }

            // Si es técnico, solo puede consultar
            // órdenes asignadas a él.
            if (User.IsInRole("Tecnico") &&
                !User.IsInRole("Administrador"))
            {
                var tecnicoId = User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

                if (orden.TecnicoId != tecnicoId)
                {
                    return Forbid();
                }
            }

            var pagos = await _pagoRepository
                .ObtenerPorOrdenAsync(id);

            var totalPagado = await _pagoRepository
                .ObtenerTotalPagadoAsync(id);

            decimal? saldoEstimado = null;

            if (orden.CostoEstimado.HasValue)
            {
                saldoEstimado =
                    orden.CostoEstimado.Value - totalPagado;
            }
            var archivos = await _archivoRepository
               .ObtenerPorOrdenAsync(id);


            ViewBag.Pagos = pagos;
            ViewBag.TotalPagado = totalPagado;
            ViewBag.SaldoEstimado = saldoEstimado;
            ViewBag.Archivos = archivos;


            return View(orden);

        }


        // ==========================================
        // EDITAR - GET
        // ==========================================

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Editar(int id)
        {
            var orden = await _ordenRepository.ObtenerPorIdAsync(id);

            if (orden == null)
            {
                return NotFound();
            }

            var viewModel = new OrdenReparacionFormViewModel
            {
                Id = orden.Id,

                EquipoId = orden.EquipoId,

                EquipoDescripcion =
                    $"{orden.Equipo.Marca} {orden.Equipo.Modelo} - " +
                    $"{orden.Equipo.Tipo} - Cliente: " +
                    $"{orden.Equipo.Cliente.Nombre} {orden.Equipo.Cliente.Apellido}",

                TecnicoId = orden.TecnicoId,

                TecnicoNombre = orden.Tecnico != null
                    ? $"{orden.Tecnico.Nombre} {orden.Tecnico.Apellido}"
                    : null,

                Problema = orden.Problema,
                Diagnostico = orden.Diagnostico,
                Estado = orden.Estado,
                FechaFinalizacion = orden.FechaFinalizacion,
                CostoEstimado = orden.CostoEstimado
            };

            return View(viewModel);
        }
        // ==========================================
        // EDITAR - POST
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Editar(
            int id,
            OrdenReparacionFormViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var orden = await _ordenRepository.ObtenerPorIdAsync(id);

            if (orden == null)
            {
                return NotFound();
            }

            // Verificar que el equipo seleccionado exista.
            var equipo = await _equipoRepository
                .ObtenerPorIdAsync(viewModel.EquipoId);

            if (equipo == null)
            {
                ModelState.AddModelError(
                    nameof(viewModel.EquipoId),
                    "El equipo seleccionado no existe.");

                return View(viewModel);
            }

            // Si hay técnico seleccionado, verificarlo.
            if (!string.IsNullOrWhiteSpace(viewModel.TecnicoId))
            {
                var tecnico = await _userManager
                    .FindByIdAsync(viewModel.TecnicoId);

                if (tecnico == null)
                {
                    ModelState.AddModelError(
                        nameof(viewModel.TecnicoId),
                        "El técnico seleccionado no existe.");

                    return View(viewModel);
                }

                var esTecnico = await _userManager
                    .IsInRoleAsync(tecnico, "Tecnico");

                if (!esTecnico)
                {
                    ModelState.AddModelError(
                        nameof(viewModel.TecnicoId),
                        "El usuario seleccionado no tiene el rol Técnico.");

                    return View(viewModel);
                }
            }

            // Actualizamos solamente los datos permitidos.
            orden.EquipoId = viewModel.EquipoId;
            orden.TecnicoId = viewModel.TecnicoId;
            orden.Problema = viewModel.Problema;
            orden.Diagnostico = viewModel.Diagnostico;
            orden.Estado = viewModel.Estado;
            orden.FechaFinalizacion = viewModel.FechaFinalizacion;
            orden.CostoEstimado = viewModel.CostoEstimado;

            await _ordenRepository.ActualizarAsync(orden);

            TempData["Mensaje"] =
                "Orden de reparación actualizada correctamente.";

            return RedirectToAction(
                nameof(Detalle),
                new { id = orden.Id });
        }
        // ==========================================
        // ACTUALIZAR TRABAJO - GET
        // Solo Técnico
        // ==========================================

        [HttpGet]
        [Authorize(Roles = "Tecnico")]
        public async Task<IActionResult> ActualizarTrabajo(int id)
        {
            var orden = await _ordenRepository.ObtenerPorIdAsync(id);

            if (orden == null)
            {
                return NotFound();
            }

            var tecnicoId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            // El técnico solo puede trabajar
            // sobre órdenes asignadas a él.
            if (orden.TecnicoId != tecnicoId)
            {
                return Forbid();
            }

            var viewModel = new OrdenTecnicoViewModel
            {
                Id = orden.Id,
                Diagnostico = orden.Diagnostico,
                Estado = orden.Estado
            };

            return View(viewModel);
        }
        // ==========================================
        // ACTUALIZAR TRABAJO - POST
        // Solo Técnico
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Tecnico")]
        public async Task<IActionResult> ActualizarTrabajo(
            int id,
            OrdenTecnicoViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var orden = await _ordenRepository.ObtenerPorIdAsync(id);

            if (orden == null)
            {
                return NotFound();
            }

            var tecnicoId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            // Seguridad:
            // solamente puede modificar órdenes asignadas a él.
            if (orden.TecnicoId != tecnicoId)
            {
                return Forbid();
            }

            orden.Diagnostico = viewModel.Diagnostico;
            orden.Estado = viewModel.Estado;

            // Fecha automática según el estado.
            if (viewModel.Estado == EstadoOrden.Reparado)
            {
                orden.FechaFinalizacion = DateTime.Now;
            }
            else if (viewModel.Estado == EstadoOrden.Entregado)
            {
                orden.FechaFinalizacion ??= DateTime.Now;
            }
            else
            {
                orden.FechaFinalizacion = null;
            }

            await _ordenRepository.ActualizarAsync(orden);

            TempData["Mensaje"] =
                "Trabajo actualizado correctamente.";

            return RedirectToAction(
                nameof(Detalle),
                new { id = orden.Id });
        }

        // ==========================================
        // CREAR - GET
        // ==========================================

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult Crear()
        {
            return View(new OrdenReparacionFormViewModel());
        }

        // ==========================================
        // CREAR - POST
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Crear(
            OrdenReparacionFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var equipo = await _equipoRepository
                .ObtenerPorIdAsync(viewModel.EquipoId);

            if (equipo == null)
            {
                ModelState.AddModelError(
                    nameof(viewModel.EquipoId),
                    "El equipo seleccionado no existe.");

                return View(viewModel);
            }

            if (!string.IsNullOrWhiteSpace(viewModel.TecnicoId))
            {
                var tecnico = await _userManager
                    .FindByIdAsync(viewModel.TecnicoId);

                if (tecnico == null)
                {
                    ModelState.AddModelError(
                        nameof(viewModel.TecnicoId),
                        "El técnico seleccionado no existe.");

                    return View(viewModel);
                }

                var esTecnico = await _userManager
                    .IsInRoleAsync(tecnico, "Tecnico");

                if (!esTecnico)
                {
                    ModelState.AddModelError(
                        nameof(viewModel.TecnicoId),
                        "El usuario seleccionado no tiene el rol Técnico.");

                    return View(viewModel);
                }
            }

            var orden = new OrdenReparacion
            {
                EquipoId = viewModel.EquipoId,
                TecnicoId = viewModel.TecnicoId,
                Problema = viewModel.Problema,
                Diagnostico = viewModel.Diagnostico,
                Estado = viewModel.Estado,
                FechaIngreso = DateTime.Now,
                FechaFinalizacion = viewModel.FechaFinalizacion,
                CostoEstimado = viewModel.CostoEstimado
            };

            await _ordenRepository.AgregarAsync(orden);

            TempData["Mensaje"] =
                "Orden de reparación registrada correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // ==========================================
        // BÚSQUEDA AJAX DE EQUIPOS
        // ==========================================

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> BuscarEquipos(
            string? buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
            {
                return Json(Array.Empty<object>());
            }

            var equipos = await _equipoRepository.BuscarAsync(
                buscar,
                pagina: 1,
                cantidadPorPagina: 10);

            var resultado = equipos.Select(e => new
            {
                id = e.Id,
                tipo = e.Tipo,
                marca = e.Marca,
                modelo = e.Modelo,
                numeroSerie = e.NumeroSerie,
                cliente = $"{e.Cliente.Nombre} {e.Cliente.Apellido}",
                documentoCliente = e.Cliente.Documento
            });

            return Json(resultado);
        }


        // ==========================================
        // BÚSQUEDA AJAX DE TÉCNICOS
        // ==========================================

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> BuscarTecnicos(
            string? buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
            {
                return Json(Array.Empty<object>());
            }

            var tecnicos =
                await _userManager.GetUsersInRoleAsync("Tecnico");

            var texto = buscar.Trim();

            var resultado = tecnicos
                .Where(t =>
                    t.Nombre.Contains(
                        texto,
                        StringComparison.OrdinalIgnoreCase) ||

                    t.Apellido.Contains(
                        texto,
                        StringComparison.OrdinalIgnoreCase) ||

                    (t.Email != null &&
                     t.Email.Contains(
                         texto,
                         StringComparison.OrdinalIgnoreCase))
                )
                .Take(10)
                .Select(t => new
                {
                    id = t.Id,
                    nombre = t.Nombre,
                    apellido = t.Apellido,
                    email = t.Email
                });

            return Json(resultado);
        }
    }
}