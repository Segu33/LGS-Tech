using System.ComponentModel.DataAnnotations;

namespace LGS.Tech.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(60)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(60)]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El documento es obligatorio.")]
        [StringLength(20)]
        public string Documento { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(30)]
        public string Telefono { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El correo ingresado no es válido.")]
        [StringLength(100)]
        public string? Correo { get; set; }

        public ICollection<Equipo> Equipos { get; set; } = new List<Equipo>();
    }
}