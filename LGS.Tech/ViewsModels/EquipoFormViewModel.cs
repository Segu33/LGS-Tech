using System.ComponentModel.DataAnnotations;

namespace LGS.Tech.ViewModels
{
    public class EquipoFormViewModel
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente.")]
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

        // Solo se usa para mostrar qué cliente fue seleccionado.
        public string? ClienteNombre { get; set; }
    }
}