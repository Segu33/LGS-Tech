using System.Text;
using LGS.Tech.Data;
using LGS.Tech.Models;
using LGS.Tech.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------
// BASE DE DATOS
// ----------------------------------------------------

var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "No se encontró la cadena de conexión 'DefaultConnection'.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    )
);

// ----------------------------------------------------
// IDENTITY
// ----------------------------------------------------

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// ----------------------------------------------------
// JWT
// ----------------------------------------------------

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "No se configuró Jwt:Key.");

var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException(
        "No se configuró Jwt:Issuer.");

var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException(
        "No se configuró Jwt:Audience.");

builder.Services
    .AddAuthentication()
    .AddJwtBearer(
        JwtBearerDefaults.AuthenticationScheme,
        options =>
        {
            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,

                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtKey)
                        ),

                    ClockSkew = TimeSpan.Zero
                };
        }
    );

// ----------------------------------------------------
// REPOSITORIOS
// ----------------------------------------------------

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IEquipoRepository, EquipoRepository>();
builder.Services.AddScoped<IOrdenReparacionRepository, OrdenReparacionRepository>();
builder.Services.AddScoped<IPagoRepository, PagoRepository>();
builder.Services.AddScoped<IArchivoOrdenRepository, ArchivoOrdenRepository>();

// ----------------------------------------------------
// MVC
// ----------------------------------------------------

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ----------------------------------------------------
// CREACIÓN DE ROLES Y USUARIOS INICIALES
// ----------------------------------------------------

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<ApplicationUser>>();

    // -----------------------------
    // Crear roles
    // -----------------------------

    string[] roles = { "Administrador", "Tecnico" };

    foreach (var rol in roles)
    {
        if (!await roleManager.RoleExistsAsync(rol))
        {
            await roleManager.CreateAsync(
                new IdentityRole(rol)
            );
        }
    }

    // -----------------------------
    // Usuario Administrador
    // -----------------------------

    string emailAdmin = "admin@lgstech.com";

    string passwordAdmin =
        builder.Configuration["SeedUsers:AdminPassword"]
        ?? throw new InvalidOperationException(
            "No se configuró SeedUsers:AdminPassword.");

    var admin = await userManager.FindByEmailAsync(emailAdmin);

    if (admin == null)
    {
        admin = new ApplicationUser
        {
            UserName = emailAdmin,
            Email = emailAdmin,
            Nombre = "Administrador",
            Apellido = "LGS",
            EmailConfirmed = true
        };

        var resultado = await userManager.CreateAsync(
            admin,
            passwordAdmin
        );

        if (resultado.Succeeded)
        {
            await userManager.AddToRoleAsync(
                admin,
                "Administrador"
            );
        }
    }

    // -----------------------------
    // Usuario Técnico
    // -----------------------------

    string emailTecnico = "tecnico@lgstech.com";

    string passwordTecnico =
        builder.Configuration["SeedUsers:TecnicoPassword"]
        ?? throw new InvalidOperationException(
            "No se configuró SeedUsers:TecnicoPassword.");

    var tecnico = await userManager.FindByEmailAsync(
        emailTecnico
    );

    if (tecnico == null)
    {
        tecnico = new ApplicationUser
        {
            UserName = emailTecnico,
            Email = emailTecnico,
            Nombre = "Tecnico",
            Apellido = "Prueba",
            EmailConfirmed = true
        };

        var resultado = await userManager.CreateAsync(
            tecnico,
            passwordTecnico
        );

        if (resultado.Succeeded)
        {
            await userManager.AddToRoleAsync(
                tecnico,
                "Tecnico"
            );
        }
    }
}

// ----------------------------------------------------
// PIPELINE HTTP
// ----------------------------------------------------

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// IMPORTANTE:
// primero autenticación, después autorización.
app.UseAuthentication();
app.UseAuthorization();

// ----------------------------------------------------
// RUTAS
// ----------------------------------------------------

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();