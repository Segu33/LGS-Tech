using System.ComponentModel.DataAnnotations;
using LGS.Tech.Models;

namespace LGS.Tech.ViewModels
{
    public class PagoFormViewModel
    {
        public int Id { get; set; }

        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "La orden de reparación no es válida."
        )]
        public int OrdenReparacionId { get; set; }

        [Range(
            0.01,
            9999999999.99,
            ErrorMessage = "El monto debe ser mayor a cero."
        )]
        public decimal Monto { get; set; }

        [Required(
            ErrorMessage = "Debe seleccionar un método de pago."
        )]
        public MetodoPago MetodoPago { get; set; }

        [Required(
            ErrorMessage = "Debe seleccionar un tipo de pago."
        )]
        public TipoPago TipoPago { get; set; }

        [StringLength(500)]
        public string? Observacion { get; set; }
    }
}