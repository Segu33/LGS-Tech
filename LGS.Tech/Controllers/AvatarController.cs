using LGS.Tech.Models;
using LGS.Tech.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LGS.Tech.Controllers;

[Authorize(Roles = "Administrador,Tecnico")]
public class AvatarController : Controller
{
    private readonly UserManager<ApplicationUser> _usuarios;
    private readonly IWebHostEnvironment _environment;

    public AvatarController(UserManager<ApplicationUser> usuarios, IWebHostEnvironment environment)
    {
        _usuarios = usuarios;
        _environment = environment;
    }

    [HttpGet("/Perfil/Avatar", Name = "MiAvatar")]
    [HttpGet("/Usuarios/CambiarAvatar/{id}", Name = "AvatarUsuario")]
    public async Task<IActionResult> CambiarAvatar([FromRoute] string? id)
    {
        if (id != null && !User.IsInRole("Administrador"))
            return Forbid();

        var usuario = id == null
            ? await _usuarios.GetUserAsync(User)
            : await _usuarios.FindByIdAsync(id);

        return usuario == null ? NotFound() : Formulario(usuario);
    }

    [HttpPost("/Perfil/Avatar")]
    [HttpPost("/Usuarios/CambiarAvatar/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Guardar([FromRoute] string? id, IFormFile? avatar)
    {
        if (id != null && !User.IsInRole("Administrador"))
            return Forbid();

        var usuario = id == null
            ? await _usuarios.GetUserAsync(User)
            : await _usuarios.FindByIdAsync(id);

        if (usuario == null)
            return NotFound();

        var extension = Path.GetExtension(avatar?.FileName)?.ToLowerInvariant();
        if (avatar == null || avatar.Length == 0)
            ModelState.AddModelError("Avatar", "Debe seleccionar una imagen.");
        else if (avatar.Length > 5 * 1024 * 1024)
            ModelState.AddModelError("Avatar", "La imagen no puede superar los 5 MB.");
        else if (extension is not (".jpg" or ".jpeg" or ".png"))
            ModelState.AddModelError("Avatar", "Solo se permiten imágenes JPG, JPEG o PNG.");

        if (!ModelState.IsValid || avatar == null)
            return Formulario(usuario);

        var carpeta = Path.Combine(_environment.WebRootPath, "uploads", "avatars");
        Directory.CreateDirectory(carpeta);
        var nombre = $"{Guid.NewGuid()}{extension}";
        var ruta = Path.Combine(carpeta, nombre);
        var anterior = usuario.Avatar;
        try
        {
            using (var archivo = new FileStream(ruta, FileMode.CreateNew))
                await avatar.CopyToAsync(archivo);

            usuario.Avatar = $"/uploads/avatars/{nombre}";
            var resultado = await _usuarios.UpdateAsync(usuario);
            if (!resultado.Succeeded)
            {
                usuario.Avatar = anterior;
                System.IO.File.Delete(ruta);
                foreach (var error in resultado.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return Formulario(usuario);
            }
        }
        catch
        {
            System.IO.File.Delete(ruta);
            throw;
        }

        if (anterior != null && anterior == "/uploads/avatars/" + Path.GetFileName(anterior))
            System.IO.File.Delete(Path.Combine(carpeta, Path.GetFileName(anterior)));

        TempData["Mensaje"] = "Avatar actualizado correctamente.";
        return id == null ? RedirectToRoute("MiAvatar") : RedirectToAction("Index", "Usuarios");
    }

    private ViewResult Formulario(ApplicationUser usuario)
    {
        return View("~/Views/Usuarios/CambiarAvatar.cshtml", new UsuarioAvatarViewModel
        {
            Id = usuario.Id,
            NombreUsuario = $"{usuario.Nombre} {usuario.Apellido}",
            AvatarActual = usuario.Avatar
        });
    }
}

