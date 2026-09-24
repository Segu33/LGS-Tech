using System.ComponentModel.DataAnnotations;

namespace LGS.Tech.Models
{
    public class ArchivoOrden
    {
        public int Id { get; set; }

        [Required]
        public int OrdenReparacionId { get; set; }

        [Required(ErrorMessage = "El nombre del archivo es obligatorio.")]
        [StringLength(255)]
        public string NombreArchivo { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string RutaArchivo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string TipoArchivo { get; set; } = string.Empty;

        public DateTime FechaSubida { get; set; } = DateTime.Now;

        public OrdenReparacion OrdenReparacion { get; set; } = null!;
    }
}