using LGS.Tech.Models;

namespace LGS.Tech.Repositories
{
    public interface IOrdenReparacionRepository
    {
        Task<List<OrdenReparacion>> BuscarAsync(
            string? buscar,
            EstadoOrden? estado,
            int pagina,
            int cantidadPorPagina,
            string? tecnicoId = null);

        Task<int> ContarAsync(
            string? buscar,
            EstadoOrden? estado,
            string? tecnicoId = null);

        Task<OrdenReparacion?> ObtenerPorIdAsync(int id);

        Task AgregarAsync(OrdenReparacion orden);

        Task ActualizarAsync(OrdenReparacion orden);

        Task EliminarAsync(OrdenReparacion orden);
    }
}