using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProyectoIS.Data;

var builder = WebApplication.CreateBuilder(args);

// Configurar Entity Framework con SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Habilitar sesiones
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// 🔹 Configurar autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";  // 🔹 Redirige al login si no está autenticado
        options.AccessDeniedPath = "/Auth/AccessDenied"; // 🔹 Página de acceso denegado
    });

builder.Services.AddAuthorization(); // 🔹 Agregar autorización

// Configurar MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

// 🔹 Habilitar autenticación y autorización en el middleware
app.UseAuthentication(); // 🔥 IMPORTANTE
app.UseAuthorization();
app.UseSession(); // 🔹 Habilita sesiones
app.UseStaticFiles(); // Asegura que las imágenes se puedan servir


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
