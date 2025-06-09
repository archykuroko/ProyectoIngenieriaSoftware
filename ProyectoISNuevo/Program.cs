using ProyectoISNuevo.Models;
using Microsoft.EntityFrameworkCore;
using ProyectoISNuevo.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Conexión a la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Sesiones
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 🔹 CORS 
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()       
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 🔹 SignalR
builder.Services.AddSignalR();

// 🔹 Autenticación (si agregas Google, completa esto más adelante)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

// 🔹 Controladores MVC + API
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 🔹 Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors();          // CORS (debe ir antes de Auth si usas credenciales)
app.UseSession();       // Sesión
app.UseAuthentication(); // Autenticación
app.UseAuthorization();  // Autorización

// 🔹 Rutas MVC (Web)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 🔹 Rutas API REST
app.MapControllers(); // para que /api/* funcione

// 🔹 SignalR Hubs
app.MapHub<ChatHub>("/chatHub");

app.Run();
