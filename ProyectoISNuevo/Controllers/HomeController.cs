using Microsoft.AspNetCore.Mvc;
using ProyectoISNuevo.Models;
using System.Collections.Generic;

namespace ProyectoISNuevo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Simulación de anuncios médicos
            var anuncios = new List<Anuncio>
            {
                new Anuncio
                {
                    Titulo = "Campaña de Vacunación",
                    Descripcion = "Acude al centro de salud más cercano para recibir tu vacuna gratuita y protegerte contra enfermedades graves.",
                    ImagenUrl = "/images/vacunacion.jpg"
                },
                new Anuncio
                {
                    Titulo = "Día Mundial de la Salud",
                    Descripcion = "Participa en nuestras charlas gratuitas sobre prevención de enfermedades, con expertos en salud pública.",
                    ImagenUrl = "/images/dia_salud.jpg"
                },
                new Anuncio
                {
                    Titulo = "Chequeos Preventivos",
                    Descripcion = "Hazte un chequeo médico anual para detectar enfermedades a tiempo. La prevención es la clave.",
                    ImagenUrl = "/images/chequeos_preventivos.jpg"
                },
                new Anuncio
                {
                    Titulo = "Terapias de Rehabilitación",
                    Descripcion = "Recibe terapia física o rehabilitación post-operatoria para recuperar tu movilidad y calidad de vida.",
                    ImagenUrl = "/images/rehabilitacion.jpg"
                },
                new Anuncio
                {
                    Titulo = "Nutrición y Dieta Saludable",
                    Descripcion = "Consulta con nuestros nutricionistas para obtener un plan de alimentación que se adapte a tus necesidades.",
                    ImagenUrl = "/images/nutricion.jpg"
                }
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
