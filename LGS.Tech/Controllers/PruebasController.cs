using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LGS.Tech.Controllers
{
    public class PruebasController : Controller
    {
        // Cualquiera puede entrar
        public IActionResult Publico()
        {
            return Content("Acceso público.");
        }

        // Solo usuarios que hayan iniciado sesión
        [Authorize]
        public IActionResult Autenticado()
        {
            return Content("Usuario autenticado.");
        }

        // Solo Administrador
        [Authorize(Roles = "Administrador")]
        public IActionResult SoloAdministrador()
        {
            return Content("Acceso permitido para Administrador.");
        }

        // Solo Técnico
        [Authorize(Roles = "Tecnico")]
        public IActionResult SoloTecnico()
        {
            return Content("Acceso permitido para Técnico.");
        }
    }
}