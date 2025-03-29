using Microsoft.EntityFrameworkCore;
using SistemaPeliculas.Data;
using SistemaPeliculas.Models;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cnx")));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


var app = builder.Build();

// Seed the database with initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
    SeedData.Initialize(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();

public static class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        // Verificar si ya existe un administrador
        if (!context.Usuarios.Any(u => u.Rol == 1))
        {
            context.Usuarios.Add(new Usuario
            {
                Nombre = "Admin",
                Apellido = "Principal",
                Email = "admin@example.com",
                Contrasena = "admin1234",
                DNI = "12345678",
                Ciudad = "Lima",
                Departamento = "Lima",
                Provincia = "Lima",
                Distrito = "Miraflores",
                Telefono = "987654321",
                EstadoCivil = "Soltero",
                Rol = 1
               

            });
            context.SaveChanges();
        }
    }
}