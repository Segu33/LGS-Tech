using LGS.Tech.Models;

namespace LGS.Tech.Repositories
{
    public interface IEquipoRepository
    {
        Task<List<Equipo>> BuscarAsync(
            string? buscar,
            int pagina,
            int cantidadPorPagina);

        Task<int> ContarAsync(string? buscar);

        Task<Equipo?> ObtenerPorIdAsync(int id);

        Task AgregarAsync(Equipo equipo);

        Task ActualizarAsync(Equipo equipo);

        Task EliminarAsync(Equipo equipo);

        Task<bool> TieneOrdenesAsync(int equipoId);
    }
}