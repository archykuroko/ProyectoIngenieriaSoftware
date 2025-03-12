using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProyectoISNuevo.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ProyectoISNuevo.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // ✅ 1. VER PERFIL
        public IActionResult Index()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Auth");

            var usuario = _context.Usuarios.Include(u => u.Rol).FirstOrDefault(u => u.Id == usuarioId);
            if (usuario == null) return NotFound();

            return View(usuario);
        }

        // ✅ 2. EDITAR PERFIL (Formulario)
        public IActionResult Edit()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Auth");

            var usuario = _context.Usuarios.Find(usuarioId);
            if (usuario == null) return NotFound();

            ViewBag.Generos = _context.Generos.ToList(); // Cargar géneros
            return View(usuario);
        }

        // ✅ 3. EDITAR PERFIL (Procesar formulario)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Usuario usuario, IFormFile? nuevaImagen)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Auth");

            var usuarioExistente = _context.Usuarios.Find(usuarioId);
            if (usuarioExistente == null) return NotFound();

            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.ApellidoPaterno = usuario.ApellidoPaterno;
            usuarioExistente.ApellidoMaterno = usuario.ApellidoMaterno;
            usuarioExistente.Genero = usuario.Genero;

            // ✅ Cambiar imagen de perfil
            if (nuevaImagen != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                string uniqueFileName = $"{usuarioExistente.Id}_{Path.GetFileName(nuevaImagen.FileName)}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    nuevaImagen.CopyTo(fileStream);
                }

                usuarioExistente.Image = $"/uploads/{uniqueFileName}";
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
