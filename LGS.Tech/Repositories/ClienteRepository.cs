using LGS.Tech.Data;
using LGS.Tech.Models;
using Microsoft.EntityFrameworkCore;

namespace LGS.Tech.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cliente>> BuscarAsync(
            string? buscar,
            int pagina,
            int cantidadPorPagina)
        {
            var consulta = _context.Clientes
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                consulta = consulta.Where(c =>
                    c.Nombre.Contains(buscar) ||
                    c.Apellido.Contains(buscar) ||
                    c.Documento.Contains(buscar));
            }

            return await consulta
                .OrderBy(c => c.Apellido)
                .ThenBy(c => c.Nombre)
                .Skip((pagina - 1) * cantidadPorPagina)
                .Take(cantidadPorPagina)
                .ToListAsync();
        }

        public async Task<int> ContarAsync(string? buscar)
        {
            var consulta = _context.Clientes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                consulta = consulta.Where(c =>
                    c.Nombre.Contains(buscar) ||
                    c.Apellido.Contains(buscar) ||
                    c.Documento.Contains(buscar));
            }

            return await consulta.CountAsync();
        }

        public async Task<Cliente?> ObtenerPorIdAsync(int id)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cliente?> ObtenerPorDocumentoAsync(string documento)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Documento == documento);
        }

        public async Task AgregarAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> TieneEquiposAsync(int clienteId)
        {
            return await _context.Equipos
                .AnyAsync(e => e.ClienteId == clienteId);
        }
    }
}