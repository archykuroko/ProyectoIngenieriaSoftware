using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoISNuevo.Models;
using System;

namespace ProyectoISNuevo.Controllers
{
    public class DatabaseStatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DatabaseStatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ 1. Verificar estado de la base de datos
        public IActionResult Index()
        {
            try
            {
                // Verificar si se puede establecer conexión con la base de datos
                _context.Database.CanConnect();

                // Contar registros en algunas tablas para verificar si la base de datos está activa
                var usuariosCount = _context.Usuarios.Count();
                var rolesCount = _context.Roles.Count();

                ViewBag.Estado = "✅ Base de datos conectada correctamente.";
                ViewBag.Usuarios = usuariosCount;
                ViewBag.Roles = rolesCount;
            }
            catch (Exception ex)
            {
                ViewBag.Estado = "❌ Error al conectar con la base de datos: " + ex.Message;
                ViewBag.Usuarios = 0;
                ViewBag.Roles = 0;
            }

            return View();
        }
    }
}
