using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ProyectoISNuevo.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            // 🔹 Verificar si el usuario está logeado
            if (HttpContext.Session.GetInt32("UsuarioId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // 🔹 Obtener datos de sesión
            string nombre = HttpContext.Session.GetString("Nombre") ?? "Nombre";
            int? rolId = HttpContext.Session.GetInt32("RolId");

            // 🔹 Determinar el nombre del rol
            string rolNombre = rolId switch
            {
                1 => "Usuario",
                2 => "Administrador",
                3 => "Doctor",
                _ => "Desconocido"
            };

            // Pasamos los datos a la vista
            ViewBag.Nombre = nombre;
            ViewBag.Rol = rolNombre;

            return View();
        }
    }
}
