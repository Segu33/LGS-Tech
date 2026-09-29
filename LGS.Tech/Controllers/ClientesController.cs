using LGS.Tech.Models;
using LGS.Tech.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LGS.Tech.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ClientesController : Controller
    {
        private readonly IClienteRepository _clienteRepository;

        public ClientesController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        // Solo devuelve la vista
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Devuelve clientes en JSON para Vue
        [HttpGet]
        public async Task<IActionResult> Listar(
            string? buscar = null,
            int pagina = 1,
            int cantidadPorPagina = 10)
        {
            if (pagina < 1)
                pagina = 1;

            if (cantidadPorPagina < 1)
                cantidadPorPagina = 10;

            var clientes = await _clienteRepository.BuscarAsync(
                buscar,
                pagina,
                cantidadPorPagina);

            var totalRegistros =
                await _clienteRepository.ContarAsync(buscar);

            var totalPaginas = (int)Math.Ceiling(
                totalRegistros / (double)cantidadPorPagina);

            return Json(new
            {
                clientes,
                paginaActual = pagina,
                totalPaginas,
                totalRegistros
            });
        }

        // Crea un cliente mediante AJAX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(
            [FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Value!.Errors
                            .Select(e => e.ErrorMessage)
                            .ToArray()
                    );

                return BadRequest(new
                {
                    mensaje = "Hay datos inválidos.",
                    errores
                });
            }

            var clienteExistente =
                await _clienteRepository
                    .ObtenerPorDocumentoAsync(
                        cliente.Documento);

            if (clienteExistente != null)
            {
                return Conflict(new
                {
                    mensaje =
                        "Ya existe un cliente con ese documento."
                });
            }

            await _clienteRepository
                .AgregarAsync(cliente);

            return Ok(new
            {
                mensaje = "Cliente creado correctamente."
            });
        }
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(
    int id,
    [FromBody] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest(new
                {
                    mensaje = "El cliente indicado no es válido."
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    mensaje = "Hay datos inválidos."
                });
            }

            var clienteActual =
                await _clienteRepository.ObtenerPorIdAsync(id);

            if (clienteActual == null)
            {
                return NotFound(new
                {
                    mensaje = "Cliente no encontrado."
                });
            }

            var clienteDocumento =
                await _clienteRepository
                    .ObtenerPorDocumentoAsync(cliente.Documento);

            if (clienteDocumento != null &&
                clienteDocumento.Id != id)
            {
                return Conflict(new
                {
                    mensaje = "Ya existe otro cliente con ese documento."
                });
            }

            clienteActual.Nombre = cliente.Nombre;
            clienteActual.Apellido = cliente.Apellido;
            clienteActual.Documento = cliente.Documento;
            clienteActual.Telefono = cliente.Telefono;
            clienteActual.Correo = cliente.Correo;

            await _clienteRepository.ActualizarAsync(clienteActual);

            return Ok(new
            {
                mensaje = "Cliente actualizado correctamente."
            });
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            var cliente = await _clienteRepository.ObtenerPorIdAsync(id);

            if (cliente == null)
            {
                return NotFound(new
                {
                    mensaje = "Cliente no encontrado."
                });
            }

            var tieneEquipos =
                await _clienteRepository.TieneEquiposAsync(id);

            if (tieneEquipos)
            {
                return Conflict(new
                {
                    mensaje = "No se puede eliminar el cliente porque tiene equipos asociados."
                });
            }

            await _clienteRepository.EliminarAsync(cliente);

            return Ok(new
            {
                mensaje = "Cliente eliminado correctamente."
            });
        }
    }
}
