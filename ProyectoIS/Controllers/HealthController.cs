using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProyectoIS.Data;

namespace ProyectoIS.Controllers
{
    public class HealthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HealthController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult CheckDatabase()
        {
            ViewBag.DatabaseStatus = CheckDatabaseStatus();
            return View();
        }

        private string CheckDatabaseStatus()
        {
            try
            {
                if (_context.Database.CanConnect())
                {
                    return "✅ Base de datos conectada correctamente";
                }
                else
                {
                    return "❌ No se pudo conectar con la base de datos.";
                }
            }
            catch (Exception ex)
            {
                return "❌ Error al conectar con la base de datos: " + ex.Message;
            }
        }
    }
}
