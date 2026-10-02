using System.ComponentModel.DataAnnotations;
using LGS.Tech.Models;

namespace LGS.Tech.ViewModels
{
    public class OrdenReparacionFormViewModel
    {
        public int Id { get; set; }

        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Debe seleccionar un equipo."
        )]
        public int EquipoId { get; set; }

        public string? TecnicoId { get; set; }

        [Required(
            ErrorMessage = "Debe indicar el problema del equipo."
        )]
        [StringLength(500)]
        public string Problema { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Diagnostico { get; set; }

        public EstadoOrden Estado { get; set; }
            = EstadoOrden.Pendiente;

        public DateTime? FechaFinalizacion { get; set; }

        [Range(
            0,
            9999999999.99,
            ErrorMessage = "El costo estimado no puede ser negativo."
        )]
        public decimal? CostoEstimado { get; set; }

        // Solo para mostrar información en los formularios.
        public string? EquipoDescripcion { get; set; }

        public string? TecnicoNombre { get; set; }
    }
}