using LGS.Tech.Data;
using LGS.Tech.Models;
using Microsoft.EntityFrameworkCore;

namespace LGS.Tech.Repositories
{
    public class ArchivoOrdenRepository
        : IArchivoOrdenRepository
    {
        private readonly ApplicationDbContext _context;

        public ArchivoOrdenRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ArchivoOrden>> ObtenerPorOrdenAsync(
            int ordenReparacionId)
        {
            return await _context.ArchivosOrden
                .AsNoTracking()
                .Where(a =>
                    a.OrdenReparacionId == ordenReparacionId)
                .OrderByDescending(a => a.FechaSubida)
                .ToListAsync();
        }

        public async Task<ArchivoOrden?> ObtenerPorIdAsync(
            int id)
        {
            return await _context.ArchivosOrden
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AgregarAsync(
            ArchivoOrden archivo)
        {
            await _context.ArchivosOrden
                .AddAsync(archivo);

            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(
            ArchivoOrden archivo)
        {
            _context.ArchivosOrden.Remove(archivo);

            await _context.SaveChangesAsync();
        }
    }
}