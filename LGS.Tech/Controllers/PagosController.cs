using LGS.Tech.Models;
using LGS.Tech.Repositories;
using LGS.Tech.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LGS.Tech.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class PagosController : Controller
    {
        private readonly IPagoRepository _pagoRepository;
        private readonly IOrdenReparacionRepository _ordenRepository;

        public PagosController(
            IPagoRepository pagoRepository,
            IOrdenReparacionRepository ordenRepository)
        {
            _pagoRepository = pagoRepository;
            _ordenRepository = ordenRepository;
        }

        // ==========================================
        // REGISTRAR - GET
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Registrar(int ordenId)
        {
            var orden = await _ordenRepository
                .ObtenerPorIdAsync(ordenId);

            if (orden == null)
            {
                return NotFound();
            }

            var viewModel = new PagoFormViewModel
            {
                OrdenReparacionId = orden.Id
            };

            ViewBag.Orden = orden;

            return View(viewModel);
        }

        // ==========================================
        // REGISTRAR - POST
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(
            PagoFormViewModel viewModel)
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

            var pago = new Pago
            {
                OrdenReparacionId = viewModel.OrdenReparacionId,
                Monto = viewModel.Monto,
                FechaPago = DateTime.Now,
                MetodoPago = viewModel.MetodoPago,
                TipoPago = viewModel.TipoPago,
                Observacion = viewModel.Observacion
            };

            await _pagoRepository.AgregarAsync(pago);

            TempData["Mensaje"] =
                "Pago registrado correctamente.";

            return RedirectToAction(
                "Detalle",
                "OrdenesReparacion",
                new { id = viewModel.OrdenReparacionId });
        }

        // ==========================================
        // ELIMINAR
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            var pago = await _pagoRepository.ObtenerPorIdAsync(id);

            if (pago == null)
            {
                return NotFound();
            }

            var ordenId = pago.OrdenReparacionId;

            await _pagoRepository.EliminarAsync(pago);

            TempData["Mensaje"] =
                "Pago eliminado correctamente.";

            return RedirectToAction(
                "Detalle",
                "OrdenesReparacion",
                new { id = ordenId });
        }
    }
}