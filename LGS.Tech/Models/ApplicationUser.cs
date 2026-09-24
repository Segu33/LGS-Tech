using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LGS.Tech.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(60)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(60)]
        public string Apellido { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Avatar { get; set; }

        public ICollection<OrdenReparacion> OrdenesAsignadas { get; set; }
            = new List<OrdenReparacion>();
    }
}