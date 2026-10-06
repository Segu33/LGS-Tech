using System.ComponentModel.DataAnnotations;

namespace LGS.Tech.ViewModels
{
    public class UsuarioPasswordViewModel
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string NuevaPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe confirmar la nueva contraseña.")]
        [DataType(DataType.Password)]
        [Compare(
            nameof(NuevaPassword),
            ErrorMessage = "Las contraseñas no coinciden."
        )]
        public string ConfirmarPassword { get; set; } = string.Empty;
    }
}