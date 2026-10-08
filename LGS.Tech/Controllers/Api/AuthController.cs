using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LGS.Tech.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LGS.Tech.Controllers.Api;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    // POST: /api/auth/login
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginApiViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Buscar usuario por email
        var usuario = await _userManager.FindByEmailAsync(model.Email);

        if (usuario == null)
        {
            return Unauthorized(new
            {
                mensaje = "Email o contraseña incorrectos."
            });
        }

        // Verificar contraseña usando Identity
        var passwordCorrecto =
            await _userManager.CheckPasswordAsync(
                usuario,
                model.Password
            );

        if (!passwordCorrecto)
        {
            return Unauthorized(new
            {
                mensaje = "Email o contraseña incorrectos."
            });
        }

        // Obtener los roles del usuario
        var roles = await _userManager.GetRolesAsync(usuario);

        // Claims que se guardarán dentro del JWT
        var claims = new List<Claim>
        {
            new Claim(
                JwtRegisteredClaimNames.Sub,
                usuario.Id
            ),

            new Claim(
                JwtRegisteredClaimNames.Email,
                usuario.Email ?? string.Empty
            ),

            new Claim(
                ClaimTypes.NameIdentifier,
                usuario.Id
            ),

            new Claim(
                ClaimTypes.Name,
                usuario.Email ?? string.Empty
            ),

            new Claim(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString()
            )
        };

        // Agregar los roles al token
        foreach (var rol in roles)
        {
            claims.Add(
                new Claim(
                    ClaimTypes.Role,
                    rol
                )
            );
        }

        // Leer configuración JWT
        var jwtKey = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException(
                "No se configuró Jwt:Key.");

        var jwtIssuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException(
                "No se configuró Jwt:Issuer.");

        var jwtAudience = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException(
                "No se configuró Jwt:Audience.");

        // Crear clave y credenciales de firma
        var clave = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)
        );

        var credenciales = new SigningCredentials(
            clave,
            SecurityAlgorithms.HmacSha256
        );

        // El token tendrá una duración de 2 horas
        var fechaExpiracion = DateTime.UtcNow.AddHours(2);

        // Crear el JWT
        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: fechaExpiracion,
            signingCredentials: credenciales
        );

        // Convertir el token a texto
        var tokenTexto =
            new JwtSecurityTokenHandler()
                .WriteToken(token);

        // Respuesta que recibirá Postman
        return Ok(new
        {
            token = tokenTexto,
            expiracion = fechaExpiracion,

            usuario = new
            {
                usuario.Id,
                usuario.Nombre,
                usuario.Apellido,
                usuario.Email,
                roles
            }
        });
    }
}

public class LoginApiViewModel
{
    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Password { get; set; } = string.Empty;
}