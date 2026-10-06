using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LGS.Tech.ViewModels
{
    public class ArchivoOrdenFormViewModel
    {
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "La orden de reparación no es válida."
        )]
        public int OrdenReparacionId { get; set; }

        [Required(
            ErrorMessage = "Debe seleccionar un archivo."
        )]
        public IFormFile? Archivo { get; set; }
    }
}