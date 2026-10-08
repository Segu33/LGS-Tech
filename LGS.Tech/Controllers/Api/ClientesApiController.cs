using LGS.Tech.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LGS.Tech.Controllers.Api;

[ApiController]
[Route("api/clientes")]
[Authorize(
    AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
    Roles = "Administrador"
)]
public class ClientesApiController : ControllerBase
{
    private readonly IClienteRepository _clienteRepository;

    public ClientesApiController(
        IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    // GET: /api/clientes
    [HttpGet]
    public async Task<IActionResult> ObtenerTodos(
     [FromQuery] string? buscar = null,
     [FromQuery] int pagina = 1,
     [FromQuery] int cantidadPorPagina = 20)
    {
        if (pagina < 1)
        {
            return BadRequest(new
            {
                mensaje = "La página debe ser mayor o igual a 1."
            });
        }

        if (cantidadPorPagina < 1 || cantidadPorPagina > 100)
        {
            return BadRequest(new
            {
                mensaje = "La cantidad por página debe estar entre 1 y 100."
            });
        }

        var clientes = await _clienteRepository.BuscarAsync(
            buscar, pagina, cantidadPorPagina);

        var total = await _clienteRepository.ContarAsync(buscar);

        return Ok(new
        {
            clientes,
            total,
            pagina,
            cantidadPorPagina,
            totalPaginas = (int)Math.Ceiling(
                (double)total / cantidadPorPagina)
        });
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        if (id < 1)
        {
            return BadRequest(new
            {
                mensaje = "El ID debe ser mayor o igual a 1."
            });
        }

        var cliente = await _clienteRepository.ObtenerPorIdAsync(id);

        if (cliente == null)
        {
            return NotFound(new
            {
                mensaje = "No se encontró el cliente."
            });
        }

        return Ok(cliente);
    }

}