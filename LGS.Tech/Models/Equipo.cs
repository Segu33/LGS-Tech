using System.ComponentModel.DataAnnotations;

namespace LGS.Tech.Models
{
    public class Equipo
    {
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "El tipo de equipo es obligatorio.")]
        [StringLength(30)]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La marca es obligatoria.")]
        [StringLength(50)]
        public string Marca { get; set; } = string.Empty;

        [Required(ErrorMessage = "El modelo es obligatorio.")]
        [StringLength(60)]
        public string Modelo { get; set; } = string.Empty;

        [StringLength(100)]
        public string? NumeroSerie { get; set; }

        public Cliente Cliente { get; set; } = null!;

        public ICollection<OrdenReparacion> OrdenesReparacion { get; set; }
            = new List<OrdenReparacion>();
    }
}