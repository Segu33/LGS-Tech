using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LGS.Tech.ViewModels
{
    public class UsuarioAvatarViewModel
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        public string NombreUsuario { get; set; } = string.Empty;

        public string? AvatarActual { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una imagen.")]
        public IFormFile? Avatar { get; set; }
    }
}