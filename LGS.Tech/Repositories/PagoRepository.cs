using LGS.Tech.Data;
using LGS.Tech.Models;
using Microsoft.EntityFrameworkCore;

namespace LGS.Tech.Repositories
{
    public class PagoRepository : IPagoRepository
    {
        private readonly ApplicationDbContext _context;

        public PagoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Pago>> ObtenerPorOrdenAsync(
            int ordenReparacionId)
        {
            return await _context.Pagos
                .AsNoTracking()
                .Where(p =>
                    p.OrdenReparacionId == ordenReparacionId)
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();
        }

        public async Task<Pago?> ObtenerPorIdAsync(int id)
        {
            return await _context.Pagos
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AgregarAsync(Pago pago)
        {
            await _context.Pagos.AddAsync(pago);

            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(Pago pago)
        {
            _context.Pagos.Remove(pago);

            await _context.SaveChangesAsync();
        }

        public async Task<decimal> ObtenerTotalPagadoAsync(
            int ordenReparacionId)
        {
            return await _context.Pagos
                .Where(p =>
                    p.OrdenReparacionId == ordenReparacionId)
                .Select(p => (decimal?)p.Monto)
                .SumAsync()
                ?? 0;
        }
    }
}