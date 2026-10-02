using System.ComponentModel.DataAnnotations;
using LGS.Tech.Models;

namespace LGS.Tech.ViewModels
{
    public class OrdenTecnicoViewModel
    {
        public int Id { get; set; }

        [StringLength(1000)]
        public string? Diagnostico { get; set; }

        [Required]
        public EstadoOrden Estado { get; set; }
    }
}