using Microsoft.AspNetCore.Mvc;

namespace ProyectoIS.Controllers
{
    public class CitaController : Controller
    {
        public IActionResult BuscarCita()
        {
            return View();
        }
    }
}
