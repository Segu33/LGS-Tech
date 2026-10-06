using LGS.Tech.Models;

namespace LGS.Tech.Repositories
{
    public interface IArchivoOrdenRepository
    {
        Task<List<ArchivoOrden>> ObtenerPorOrdenAsync(
            int ordenReparacionId);

        Task<ArchivoOrden?> ObtenerPorIdAsync(int id);

        Task AgregarAsync(ArchivoOrden archivo);

        Task EliminarAsync(ArchivoOrden archivo);
    }
}