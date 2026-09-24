using System.ComponentModel.DataAnnotations;

namespace LGS.Tech.Models
{
    public enum MetodoPago
    {
        Efectivo,
        Transferencia,
        Tarjeta
    }

    public enum TipoPago
    {
        Seña,
        Parcial,
        Total
    }

    public class Pago
    {
        public int Id { get; set; }

        [Required]
        public int OrdenReparacionId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Monto { get; set; }

        public DateTime FechaPago { get; set; } = DateTime.Now;

        [Required]
        public MetodoPago MetodoPago { get; set; }

        [Required]
        public TipoPago TipoPago { get; set; }

        public string? Observacion { get; set; }

        public OrdenReparacion OrdenReparacion { get; set; } = null!;
    }
}