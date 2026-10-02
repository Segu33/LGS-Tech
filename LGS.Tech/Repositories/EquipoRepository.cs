using LGS.Tech.Data;
using LGS.Tech.Models;
using Microsoft.EntityFrameworkCore;

namespace LGS.Tech.Repositories
{
    public class EquipoRepository : IEquipoRepository
    {
        private readonly ApplicationDbContext _context;

        public EquipoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Equipo>> BuscarAsync(
            string? buscar,
            int pagina,
            int cantidadPorPagina)
        {
            var consulta = _context.Equipos
                .Include(e => e.Cliente)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                consulta = consulta.Where(e =>
                    e.Tipo.Contains(buscar) ||
                    e.Marca.Contains(buscar) ||
                    e.Modelo.Contains(buscar) ||
                    (e.NumeroSerie != null &&
                     e.NumeroSerie.Contains(buscar)) ||
                    e.Cliente.Nombre.Contains(buscar) ||
                    e.Cliente.Apellido.Contains(buscar) ||
                    e.Cliente.Documento.Contains(buscar));
            }

            return await consulta
                .OrderBy(e => e.Marca)
                .ThenBy(e => e.Modelo)
                .Skip((pagina - 1) * cantidadPorPagina)
                .Take(cantidadPorPagina)
                .ToListAsync();
        }

        public async Task<int> ContarAsync(string? buscar)
        {
            var consulta = _context.Equipos
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                consulta = consulta.Where(e =>
                    e.Tipo.Contains(buscar) ||
                    e.Marca.Contains(buscar) ||
                    e.Modelo.Contains(buscar) ||
                    (e.NumeroSerie != null &&
                     e.NumeroSerie.Contains(buscar)) ||
                    e.Cliente.Nombre.Contains(buscar) ||
                    e.Cliente.Apellido.Contains(buscar) ||
                    e.Cliente.Documento.Contains(buscar));
            }

            return await consulta.CountAsync();
        }

        public async Task<Equipo?> ObtenerPorIdAsync(int id)
        {
            return await _context.Equipos
                .Include(e => e.Cliente)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AgregarAsync(Equipo equipo)
        {
            await _context.Equipos.AddAsync(equipo);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Equipo equipo)
        {
            _context.Equipos.Update(equipo);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(Equipo equipo)
        {
            _context.Equipos.Remove(equipo);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> TieneOrdenesAsync(int equipoId)
        {
            return await _context.OrdenesReparacion
                .AnyAsync(o => o.EquipoId == equipoId);
        }
    }
}