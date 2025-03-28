using Microsoft.AspNetCore.Mvc;
using ProyectoISNuevo.Models;
using System.Collections.Generic;

namespace ProyectoISNuevo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // 🔹 Simulación de anuncios médicos
            var anuncios = new List<dynamic>
            {
                new { Titulo = "Campaña de Vacunación", Descripcion = "Acude al centro de salud más cercano para recibir tu vacuna gratuita." },
                new { Titulo = "Día Mundial de la Salud", Descripcion = "Participa en nuestras charlas gratuitas sobre prevención de enfermedades." },
                new { Titulo = "Chequeos Preventivos", Descripcion = "Hazte un chequeo médico anual para detectar enfermedades a tiempo." }
            };

            ViewBag.Anuncios = anuncios;
            return View();
        }

        //Política de Privacidad
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
