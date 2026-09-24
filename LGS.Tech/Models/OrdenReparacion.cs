using System.ComponentModel.DataAnnotations;

namespace LGS.Tech.Models
{
    public class OrdenReparacion
    {
        public int Id { get; set; }

        [Required]
        public int EquipoId { get; set; }

        public string? TecnicoId { get; set; }

        [Required(ErrorMessage = "El problema del equipo es obligatorio.")]
        [StringLength(500)]
        public string Problema { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Diagnostico { get; set; }

        [Required]
        public EstadoOrden Estado { get; set; } = EstadoOrden.Pendiente;

        public DateTime FechaIngreso { get; set; } = DateTime.Now;

        public DateTime? FechaFinalizacion { get; set; }

        public decimal? CostoEstimado { get; set; }

        // Navegación hacia Equipo
        public Equipo Equipo { get; set; } = null!;

        // Más adelante:
        // public ApplicationUser? Tecnico { get; set; }

        public ICollection<ArchivoOrden> Archivos { get; set; }
        = new List<ArchivoOrden>();

        public ICollection<Pago> Pagos { get; set; }
        = new List<Pago>();
    }
}