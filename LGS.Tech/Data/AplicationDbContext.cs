using LGS.Tech.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LGS.Tech.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<OrdenReparacion> OrdenesReparacion { get; set; }
        public DbSet<ArchivoOrden> ArchivosOrden { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Cliente>()
                .HasIndex(c => c.Documento)
                .IsUnique();

            builder.Entity<Equipo>()
                .HasOne(e => e.Cliente)
                .WithMany(c => c.Equipos)
                .HasForeignKey(e => e.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrdenReparacion>()
                .HasOne(o => o.Equipo)
                .WithMany(e => e.OrdenesReparacion)
                .HasForeignKey(o => o.EquipoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrdenReparacion>()
                .HasOne(o => o.Tecnico)
                .WithMany(u => u.OrdenesAsignadas)
                .HasForeignKey(o => o.TecnicoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ArchivoOrden>()
                .HasOne(a => a.OrdenReparacion)
                .WithMany(o => o.Archivos)
                .HasForeignKey(a => a.OrdenReparacionId);

            builder.Entity<Pago>()
                .HasOne(p => p.OrdenReparacion)
                .WithMany(o => o.Pagos)
                .HasForeignKey(p => p.OrdenReparacionId);

            // Precisión para valores monetarios
            builder.Entity<OrdenReparacion>()
                .Property(o => o.CostoEstimado)
                .HasPrecision(12, 2);

            builder.Entity<Pago>()
                .Property(p => p.Monto)
                .HasPrecision(12, 2);
        }
    }
}