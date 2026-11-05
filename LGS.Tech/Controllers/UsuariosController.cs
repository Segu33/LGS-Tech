using LGS.Tech.Models;
using LGS.Tech.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LGS.Tech.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuariosController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // ==========================================
        // LISTADO
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarios = await _userManager.Users
                .OrderBy(u => u.Apellido)
                .ThenBy(u => u.Nombre)
                .ToListAsync();

            var listado = new List<UsuarioListadoViewModel>();

            foreach (var usuario in usuarios)
            {
                var roles = await _userManager
                    .GetRolesAsync(usuario);

                listado.Add(new UsuarioListadoViewModel
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Email = usuario.Email ?? string.Empty,
                    Rol = roles.FirstOrDefault() ?? "Sin rol"
                });
            }

            return View(listado);
        }
        // ==========================================
        // CREAR - GET
        // ==========================================

        [HttpGet]
        public IActionResult Crear()
        {
            return View(new UsuarioCrearViewModel());
        }

        // ==========================================
        // CREAR - POST
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(
            UsuarioCrearViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Solamente permitimos los roles del sistema.
            var rolesPermitidos = new[]
            {
        "Administrador",
        "Tecnico"
    };

            if (!rolesPermitidos.Contains(viewModel.Rol))
            {
                ModelState.AddModelError(
                    nameof(viewModel.Rol),
                    "El rol seleccionado no es válido.");

                return View(viewModel);
            }

            // Verificamos que el rol realmente exista en Identity.
            if (!await _roleManager.RoleExistsAsync(viewModel.Rol))
            {
                ModelState.AddModelError(
                    nameof(viewModel.Rol),
                    "El rol seleccionado no existe.");

                return View(viewModel);
            }

            // No permitimos correos duplicados.
            var usuarioExistente = await _userManager
                .FindByEmailAsync(viewModel.Email);

            if (usuarioExistente != null)
            {
                ModelState.AddModelError(
                    nameof(viewModel.Email),
                    "Ya existe un usuario con ese correo.");

                return View(viewModel);
            }

            var usuario = new ApplicationUser
            {
                UserName = viewModel.Email,
                Email = viewModel.Email,
                Nombre = viewModel.Nombre,
                Apellido = viewModel.Apellido,
                EmailConfirmed = true
            };

            var resultado = await _userManager.CreateAsync(
                usuario,
                viewModel.Password
            );

            if (!resultado.Succeeded)
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description
                    );
                }

                return View(viewModel);
            }

            var resultadoRol = await _userManager
                .AddToRoleAsync(usuario, viewModel.Rol);

            if (!resultadoRol.Succeeded)
            {
                // Si no pudimos asignar el rol,
                // eliminamos el usuario recién creado
                // para no dejarlo inconsistente.
                await _userManager.DeleteAsync(usuario);

                foreach (var error in resultadoRol.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description
                    );
                }

                return View(viewModel);
            }

            TempData["Mensaje"] =
                "Usuario creado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        // ==========================================
        // EDITAR - GET
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Editar(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(usuario);

            var viewModel = new UsuarioEditarViewModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email ?? string.Empty,
                Rol = roles.FirstOrDefault() ?? string.Empty
            };

            return View(viewModel);
        }
        // ==========================================
        // EDITAR - POST
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(
            string id,
            UsuarioEditarViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var usuario = await _userManager.FindByIdAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }
            // ==========================================
            // IMPEDIR QUE EL ADMINISTRADOR ACTUAL
            // SE QUITE SU PROPIO ROL
            // ==========================================

            var usuarioActualId = _userManager.GetUserId(User);

            if (usuario.Id == usuarioActualId &&
                viewModel.Rol != "Administrador")
            {
                ModelState.AddModelError(
                    nameof(viewModel.Rol),
                    "No puede quitarse a sí mismo el rol Administrador."
                );

                return View(viewModel);
            }

            var rolesPermitidos = new[]
            {
        "Administrador",
        "Tecnico"
    };

            if (!rolesPermitidos.Contains(viewModel.Rol))
            {
                ModelState.AddModelError(
                    nameof(viewModel.Rol),
                    "El rol seleccionado no es válido.");

                return View(viewModel);
            }

            if (!await _roleManager.RoleExistsAsync(viewModel.Rol))
            {
                ModelState.AddModelError(
                    nameof(viewModel.Rol),
                    "El rol seleccionado no existe.");

                return View(viewModel);
            }

            var usuarioConEmail = await _userManager
                .FindByEmailAsync(viewModel.Email);

            if (usuarioConEmail != null &&
                usuarioConEmail.Id != usuario.Id)
            {
                ModelState.AddModelError(
                    nameof(viewModel.Email),
                    "Ya existe otro usuario con ese correo.");

                return View(viewModel);
            }

            usuario.Nombre = viewModel.Nombre;
            usuario.Apellido = viewModel.Apellido;
            usuario.Email = viewModel.Email;
            usuario.UserName = viewModel.Email;

            var resultadoUsuario =
                await _userManager.UpdateAsync(usuario);

            if (!resultadoUsuario.Succeeded)
            {
                foreach (var error in resultadoUsuario.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return View(viewModel);
            }

            var rolesActuales =
                await _userManager.GetRolesAsync(usuario);

            if (!rolesActuales.Contains(viewModel.Rol))
            {
                if (rolesActuales.Count > 0)
                {
                    var resultadoQuitar =
                        await _userManager.RemoveFromRolesAsync(
                            usuario,
                            rolesActuales);

                    if (!resultadoQuitar.Succeeded)
                    {
                        foreach (var error in resultadoQuitar.Errors)
                        {
                            ModelState.AddModelError(
                                string.Empty,
                                error.Description);
                        }

                        return View(viewModel);
                    }
                }

                var resultadoAgregar =
                    await _userManager.AddToRoleAsync(
                        usuario,
                        viewModel.Rol);

                if (!resultadoAgregar.Succeeded)
                {
                    foreach (var error in resultadoAgregar.Errors)
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            error.Description);
                    }

                    return View(viewModel);
                }
            }

            TempData["Mensaje"] =
                "Usuario actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        // ==========================================
        // CAMBIAR PASSWORD - GET
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> CambiarPassword(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            var viewModel = new UsuarioPasswordViewModel
            {
                Id = usuario.Id,
                NombreUsuario =
                    $"{usuario.Nombre} {usuario.Apellido}"
            };

            return View(viewModel);
        }
        // ==========================================
        // CAMBIAR PASSWORD - POST
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarPassword(
            string id,
            UsuarioPasswordViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var usuario = await _userManager.FindByIdAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            var token = await _userManager
                .GeneratePasswordResetTokenAsync(usuario);

            var resultado = await _userManager
                .ResetPasswordAsync(
                    usuario,
                    token,
                    viewModel.NuevaPassword
                );

            if (!resultado.Succeeded)
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description
                    );
                }

                viewModel.NombreUsuario =
                    $"{usuario.Nombre} {usuario.Apellido}";

                return View(viewModel);
            }

            TempData["Mensaje"] =
                "Contraseña actualizada correctamente.";

            return RedirectToAction(nameof(Index));
        }
    }
}
