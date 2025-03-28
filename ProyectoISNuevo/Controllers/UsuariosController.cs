using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProyectoISNuevo.Models;

namespace ProyectoISNuevo.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 📌 Verifica si el usuario es Administrador
        private bool EsAdministrador()
        {
            return HttpContext.Session.GetInt32("RolId") == 2;
        }

        // ✅ 1. LISTAR USUARIOS (Solo Administradores)
        public IActionResult Index()
        {
            if (!EsAdministrador()) return RedirectToAction("Index", "Dashboard");

            var usuarios = _context.Usuarios.Include(u => u.Rol).ToList();
            return View(usuarios);
        }

        // ✅ 2. CREAR USUARIO (Formulario)
        public IActionResult Create()
        {
            if (!EsAdministrador()) return RedirectToAction("Index");

            ViewBag.Roles = _context.Roles.ToList();
            return View();
        }

        // ✅ 3. CREAR USUARIO (Procesar formulario)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Usuario usuario)
        {
            if (!EsAdministrador()) return RedirectToAction("Index");

            if (_context.Usuarios.Any(u => u.Correo == usuario.Correo))
            {
                ViewBag.Error = "❌ Este correo ya está registrado.";
                ViewBag.Roles = _context.Roles.ToList();
                return View(usuario);
            }

            usuario.Contraseña = HashPassword(usuario.Contraseña);
            usuario.Image = "/uploads/default.png"; // Imagen por defecto

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ✅ 4. EDITAR USUARIO (Formulario)
        public IActionResult Edit(int id)
        {
            if (!EsAdministrador()) return RedirectToAction("Index");

            var usuario = _context.Usuarios.Find(id);
            if (usuario == null) return NotFound();

            ViewBag.Roles = _context.Roles.ToList();
            return View(usuario);
        }

        // ✅ 5. EDITAR USUARIO (Procesar cambios)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Usuario usuario)
        {
            if (!EsAdministrador()) return RedirectToAction("Index");

            var usuarioExistente = _context.Usuarios.Find(usuario.Id);
            if (usuarioExistente == null) return NotFound();

            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.ApellidoPaterno = usuario.ApellidoPaterno;
            usuarioExistente.ApellidoMaterno = usuario.ApellidoMaterno;
            usuarioExistente.Usuario1 = usuario.Usuario1;
            usuarioExistente.Correo = usuario.Correo;

            // 🔹 Actualizar el rol del usuario
            usuarioExistente.Idrol = usuario.Idrol;

            // 🔹 Si el usuario ingresó una nueva contraseña, actualizarla
            if (!string.IsNullOrEmpty(usuario.Contraseña))
            {
                usuarioExistente.Contraseña = HashPassword(usuario.Contraseña);
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        // ✅ 6. ELIMINAR USUARIO (Confirmación)
        public IActionResult Delete(int id)
        {
            if (!EsAdministrador()) return RedirectToAction("Index");

            var usuario = _context.Usuarios.Find(id);
            if (usuario == null) return NotFound();

            return View(usuario);
        }

        // ✅ 7. ELIMINAR USUARIO (Procesar eliminación)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!EsAdministrador()) return RedirectToAction("Index");

            var usuario = _context.Usuarios.Find(id);
            if (usuario == null) return NotFound();

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ✅ Método para encriptar contraseñas
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
