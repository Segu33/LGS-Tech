using LGS.Tech.Models;

namespace LGS.Tech.Repositories
{
    public interface IPagoRepository
    {
        Task<List<Pago>> ObtenerPorOrdenAsync(
            int ordenReparacionId);

        Task<Pago?> ObtenerPorIdAsync(int id);

        Task AgregarAsync(Pago pago);

        Task EliminarAsync(Pago pago);

        Task<decimal> ObtenerTotalPagadoAsync(
            int ordenReparacionId);
    }
}