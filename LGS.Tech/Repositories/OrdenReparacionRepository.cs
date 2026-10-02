using LGS.Tech.Data;
using LGS.Tech.Models;
using Microsoft.EntityFrameworkCore;

namespace LGS.Tech.Repositories
{
    public class OrdenReparacionRepository
        : IOrdenReparacionRepository
    {
        private readonly ApplicationDbContext _context;

        public OrdenReparacionRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrdenReparacion>> BuscarAsync(
            string? buscar,
            EstadoOrden? estado,
            int pagina,
            int cantidadPorPagina,
            string? tecnicoId = null)
        {
            var consulta = _context.OrdenesReparacion
                .Include(o => o.Equipo)
                    .ThenInclude(e => e.Cliente)
                .Include(o => o.Tecnico)
                .AsNoTracking()
                .AsQueryable();

            // Filtro por técnico
            if (!string.IsNullOrWhiteSpace(tecnicoId))
            {
                consulta = consulta.Where(
                    o => o.TecnicoId == tecnicoId
                );
            }

            // Filtro por estado
            if (estado.HasValue)
            {
                consulta = consulta.Where(
                    o => o.Estado == estado.Value
                );
            }

            // Búsqueda general
            if (!string.IsNullOrWhiteSpace(buscar))
            {
                consulta = consulta.Where(o =>
                    o.Problema.Contains(buscar) ||

                    (o.Diagnostico != null &&
                     o.Diagnostico.Contains(buscar)) ||

                    o.Equipo.Tipo.Contains(buscar) ||

                    o.Equipo.Marca.Contains(buscar) ||

                    o.Equipo.Modelo.Contains(buscar) ||

                    (o.Equipo.NumeroSerie != null &&
                     o.Equipo.NumeroSerie.Contains(buscar)) ||

                    o.Equipo.Cliente.Nombre.Contains(buscar) ||

                    o.Equipo.Cliente.Apellido.Contains(buscar) ||

                    o.Equipo.Cliente.Documento.Contains(buscar)
                );
            }

            return await consulta
                .OrderByDescending(o => o.FechaIngreso)
                .Skip((pagina - 1) * cantidadPorPagina)
                .Take(cantidadPorPagina)
                .ToListAsync();
        }

        public async Task<int> ContarAsync(
            string? buscar,
            EstadoOrden? estado,
            string? tecnicoId = null)
        {
            var consulta = _context.OrdenesReparacion
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(tecnicoId))
            {
                consulta = consulta.Where(
                    o => o.TecnicoId == tecnicoId
                );
            }

            if (estado.HasValue)
            {
                consulta = consulta.Where(
                    o => o.Estado == estado.Value
                );
            }

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                consulta = consulta.Where(o =>
                    o.Problema.Contains(buscar) ||

                    (o.Diagnostico != null &&
                     o.Diagnostico.Contains(buscar)) ||

                    o.Equipo.Tipo.Contains(buscar) ||

                    o.Equipo.Marca.Contains(buscar) ||

                    o.Equipo.Modelo.Contains(buscar) ||

                    (o.Equipo.NumeroSerie != null &&
                     o.Equipo.NumeroSerie.Contains(buscar)) ||

                    o.Equipo.Cliente.Nombre.Contains(buscar) ||

                    o.Equipo.Cliente.Apellido.Contains(buscar) ||

                    o.Equipo.Cliente.Documento.Contains(buscar)
                );
            }

            return await consulta.CountAsync();
        }

        public async Task<OrdenReparacion?> ObtenerPorIdAsync(
            int id)
        {
            return await _context.OrdenesReparacion
                .Include(o => o.Equipo)
                    .ThenInclude(e => e.Cliente)
                .Include(o => o.Tecnico)
                .Include(o => o.Archivos)
                .Include(o => o.Pagos)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task AgregarAsync(
            OrdenReparacion orden)
        {
            await _context.OrdenesReparacion
                .AddAsync(orden);

            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(
            OrdenReparacion orden)
        {
            _context.OrdenesReparacion.Update(orden);

            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(
            OrdenReparacion orden)
        {
            _context.OrdenesReparacion.Remove(orden);

            await _context.SaveChangesAsync();
        }
    }
}