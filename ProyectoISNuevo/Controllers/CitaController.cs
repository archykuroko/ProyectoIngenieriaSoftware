using Microsoft.AspNetCore.Mvc;
using ProyectoISNuevo.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Microsoft.EntityFrameworkCore;

namespace ProyectoISNuevo.Controllers
{
    public class CitaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CitaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult BuscarCita()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BuscarCita(string codigoCita)
        {
            if (string.IsNullOrEmpty(codigoCita))
            {
                TempData["Mensaje"] = "⚠️ Por favor, ingresa un código de cita válido.";
                return View();
            }

            // 🔹 Buscar la cita en la base de datos incluyendo las relaciones con Usuario (Paciente) y Doctor
            var cita = _context.Citas
                .Include(c => c.Usuario)
                .Include(c => c.Doctor)
                .FirstOrDefault(c => c.IdCita == codigoCita);

            if (cita == null)
            {
                TempData["Mensaje"] = "❌ No se encontró una cita con ese código.";
                return View();
            }

            // 🔹 Enviar los datos al ViewBag
            ViewBag.Cita = new
            {
                IdCita = cita.IdCita,
                Paciente = $"{cita.Usuario.Nombre} {cita.Usuario.ApellidoPaterno}",
                FechaHora = cita.FechaHora.ToString("f"),
                Motivo = cita.Motivo,
                Doctor = $"{cita.Doctor.Nombre} {cita.Doctor.ApellidoPaterno}",
                Estado = cita.Estado
            };

            return View();
        }




        // ✅ 1. LISTAR TODAS LAS CITAS (Solo Administradores)
        public IActionResult Index()
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");
            if (rolId != 2) return RedirectToAction("Index", "Dashboard");

            var citas = _context.Citas.ToList();
            return View(citas);
        }

        // ✅ 2. VER CITAS ASIGNADAS (Solo Doctores)
        public IActionResult Asignadas()
        {
            int? rolId = HttpContext.Session.GetInt32("RolId");
            if (rolId != 3) return RedirectToAction("Index", "Dashboard");

            var citas = _context.Citas.Where(c => c.Estado == "Asignada").ToList();
            return View(citas);
        }

        // ✅ 3. VER CITAS DEL USUARIO (Solo Usuarios)
        public IActionResult MisCitas()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Auth");

            var citas = _context.Citas.Where(c => c.UsuarioId == usuarioId).ToList();
            return View(citas);
        }

        // ✅ MOSTRAR FORMULARIO PARA CREAR UNA CITA
        public IActionResult Crear()
        {
            if (HttpContext.Session.GetInt32("UsuarioId") == null)
                return RedirectToAction("Login", "Auth");

            // 🔹 Obtener la lista de doctores registrados (RolId = 3)
            var doctores = _context.Usuarios
                .Where(u => u.Idrol == 3)
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = $"{u.Nombre} {u.ApellidoPaterno} {u.ApellidoMaterno}"
                })
                .ToList();

            ViewBag.Doctores = doctores;
            return View();
        }


        // ✅ PROCESAR CREACIÓN DE CITA
        [HttpPost]
        public IActionResult Crear(Cita cita, int DoctorId)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Auth");

            // Validar que la fecha seleccionada sea en el futuro
            if (cita.FechaHora < DateTime.Now)
            {
                TempData["Error"] = "❌ No puedes seleccionar una fecha pasada.";
                return RedirectToAction("Crear");
            }

            // Validar si el doctor seleccionado existe y es de rol 3
            var doctorExiste = _context.Usuarios.Any(u => u.Id == DoctorId && u.Idrol == 3);
            if (!doctorExiste)
            {
                TempData["Error"] = "❌ El doctor seleccionado no es válido.";
                return RedirectToAction("Crear");
            }

            // Generar un ID aleatorio de 6 caracteres
            cita.IdCita = GenerarCodigoCita();
            cita.UsuarioId = usuarioId.Value;
            cita.DoctorId = DoctorId; // 🔹 Se asigna el doctor seleccionado
            cita.Estado = "Pendiente";

            _context.Citas.Add(cita);
            _context.SaveChanges();

            TempData["Mensaje"] = "✅ Cita generada correctamente.";
            return RedirectToAction("MisCitas");
        }

        // ✅ MÉTODO PARA GENERAR CÓDIGO ALEATORIO DE CITA
        private string GenerarCodigoCita()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
    }
}
