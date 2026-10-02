using LGS.Tech.Models;
using LGS.Tech.Repositories;
using LGS.Tech.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LGS.Tech.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class EquiposController : Controller
    {
        private readonly IEquipoRepository _equipoRepository;
        private readonly IClienteRepository _clienteRepository;

        public EquiposController(
            IEquipoRepository equipoRepository,
            IClienteRepository clienteRepository)
        {
            _equipoRepository = equipoRepository;
            _clienteRepository = clienteRepository;
        }

        // ==========================================
        // LISTADO
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Index(
            string? buscar = null,
            int pagina = 1,
            int cantidadPorPagina = 10)
        {
            if (pagina < 1)
                pagina = 1;

            if (cantidadPorPagina < 1)
                cantidadPorPagina = 10;

            var equipos = await _equipoRepository.BuscarAsync(
                buscar,
                pagina,
                cantidadPorPagina);

            var totalRegistros =
                await _equipoRepository.ContarAsync(buscar);

            var totalPaginas = (int)Math.Ceiling(
                totalRegistros / (double)cantidadPorPagina);

            ViewBag.Buscar = buscar;
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;
            ViewBag.TotalRegistros = totalRegistros;

            return View(equipos);
        }

        // ==========================================
        // CREAR - GET
        // ==========================================

        [HttpGet]
        public IActionResult Crear()
        {
            return View(new EquipoFormViewModel());
        }

        // ==========================================
        // CREAR - POST
        // ==========================================

        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Crear(
    EquipoFormViewModel viewModel)
{
    if (!ModelState.IsValid)
    {
        return View(viewModel);
    }

    var cliente = await _clienteRepository
        .ObtenerPorIdAsync(viewModel.ClienteId);

    if (cliente == null)
    {
        ModelState.AddModelError(
            nameof(viewModel.ClienteId),
            "El cliente seleccionado no existe.");

        return View(viewModel);
    }

    var equipo = new Equipo
    {
        ClienteId = viewModel.ClienteId,
        Tipo = viewModel.Tipo,
        Marca = viewModel.Marca,
        Modelo = viewModel.Modelo,
        NumeroSerie = viewModel.NumeroSerie
    };

    await _equipoRepository.AgregarAsync(equipo);

    TempData["Mensaje"] =
        "Equipo registrado correctamente.";

    return RedirectToAction(nameof(Index));
}
// ==========================================
// EDITAR - GET
// ==========================================

[HttpGet]
public async Task<IActionResult> Editar(int id)
{
    var equipo = await _equipoRepository.ObtenerPorIdAsync(id);

    if (equipo == null)
    {
        return NotFound();
    }

    var viewModel = new EquipoFormViewModel
    {
        Id = equipo.Id,
        ClienteId = equipo.ClienteId,
        ClienteNombre =
            $"{equipo.Cliente.Nombre} {equipo.Cliente.Apellido}",
        Tipo = equipo.Tipo,
        Marca = equipo.Marca,
        Modelo = equipo.Modelo,
        NumeroSerie = equipo.NumeroSerie
    };

    return View(viewModel);
}
// ==========================================
// EDITAR - POST
// ==========================================

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Editar(
    int id,
    EquipoFormViewModel viewModel)
{
    if (id != viewModel.Id)
    {
        return BadRequest();
    }

    if (!ModelState.IsValid)
    {
        return View(viewModel);
    }

    var equipo = await _equipoRepository.ObtenerPorIdAsync(id);

    if (equipo == null)
    {
        return NotFound();
    }

    var cliente = await _clienteRepository
        .ObtenerPorIdAsync(viewModel.ClienteId);

    if (cliente == null)
    {
        ModelState.AddModelError(
            nameof(viewModel.ClienteId),
            "El cliente seleccionado no existe.");

        return View(viewModel);
    }

    equipo.ClienteId = viewModel.ClienteId;
    equipo.Tipo = viewModel.Tipo;
    equipo.Marca = viewModel.Marca;
    equipo.Modelo = viewModel.Modelo;
    equipo.NumeroSerie = viewModel.NumeroSerie;

    await _equipoRepository.ActualizarAsync(equipo);

    TempData["Mensaje"] =
        "Equipo actualizado correctamente.";

    return RedirectToAction(nameof(Index));
}
// ==========================================
// ELIMINAR
// ==========================================

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Eliminar(int id)
{
    var equipo = await _equipoRepository.ObtenerPorIdAsync(id);

    if (equipo == null)
    {
        return NotFound();
    }

    var tieneOrdenes =
        await _equipoRepository.TieneOrdenesAsync(id);

    if (tieneOrdenes)
    {
        TempData["Error"] =
            "No se puede eliminar el equipo porque tiene órdenes de reparación asociadas.";

        return RedirectToAction(nameof(Index));
    }

    await _equipoRepository.EliminarAsync(equipo);

    TempData["Mensaje"] =
        "Equipo eliminado correctamente.";

    return RedirectToAction(nameof(Index));
}

        // ==========================================
        // BÚSQUEDA AJAX DE CLIENTES
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> BuscarClientes(
            string? buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
            {
                return Json(Array.Empty<object>());
            }

            var clientes = await _clienteRepository.BuscarAsync(
                buscar,
                pagina: 1,
                cantidadPorPagina: 10);

            var resultado = clientes.Select(c => new
            {
                id = c.Id,
                nombre = c.Nombre,
                apellido = c.Apellido,
                documento = c.Documento
            });

            return Json(resultado);
        }
    }
}