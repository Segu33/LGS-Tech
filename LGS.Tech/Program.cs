using LGS.Tech.Data;
using LGS.Tech.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "No se encontró la cadena de conexión 'DefaultConnection'.");



// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    )
);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();



builder.Services.AddControllersWithViews();

var app = builder.Build();

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
            await roleManager.CreateAsync(new IdentityRole(rol));
        }
    }

    // -----------------------------
    // Usuario Administrador
    // -----------------------------
    string emailAdmin = "admin@lgstech.com";
    string passwordAdmin = "Admin123!";

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
    string passwordTecnico = "Tecnico123!";

    var tecnico = await userManager.FindByEmailAsync(emailTecnico);

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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
