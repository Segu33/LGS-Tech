using LGS.Tech.Models;

namespace LGS.Tech.Repositories
{
    public interface IClienteRepository
    {
        Task<List<Cliente>> BuscarAsync(
        string? buscar,
        int pagina,
        int cantidadPorPagina);

        Task<int> ContarAsync(string? buscar);
        Task<Cliente?> ObtenerPorIdAsync(int id);

        Task<Cliente?> ObtenerPorDocumentoAsync(string documento);

        Task AgregarAsync(Cliente cliente);

        Task ActualizarAsync(Cliente cliente);

        Task EliminarAsync(Cliente cliente);

        Task<bool> TieneEquiposAsync(int clienteId);
    }
}