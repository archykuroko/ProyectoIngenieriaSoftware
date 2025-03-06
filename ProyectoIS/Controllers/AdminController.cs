using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using ProyectoIS.Data;
using ProyectoIS.Models;
using ProyectoIS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoIS.Controllers
{
    [Authorize] // 🔒 Solo usuarios autenticados pueden acceder
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Verifica si el usuario es administrador
        private bool EsAdministrador()
        {
            var rolId = HttpContext.Session.GetInt32("RolId");
            return rolId.HasValue && rolId == 2; // 🔹 Rol 2 = Administrador
        }

        // 📌 1. LISTAR USUARIOS (Disponible para todos los roles)
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("RolId") == null)
                return RedirectToAction("Login", "Auth");

            var usuarios = _context.Usuarios
                .Select(u => new UsuarioViewModel
                {
                    Id = u.ID,
                    Usuario = u.Usuario,
                    Nombre = u.Nombre,
                    ApellidoPaterno = u.ApellidoPaterno,
                    ApellidoMaterno = u.ApellidoMaterno,
                    Correo = u.Correo,
                    RolNombre = u.Rol != null ? u.Rol.Nombre : "Sin Rol",
                    Image = u.Image
                })
                .ToList();

            return View(usuarios);
        }

        // 📌 2. MOSTRAR FORMULARIO DE CREACIÓN (Solo Administradores)
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            if (!EsAdministrador()) return RedirectToAction("Index");

            ViewBag.Roles = _context.Roles
                .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Nombre })
                .ToList();

            return View();
        }

        // 📌 3. PROCESAR CREACIÓN (Solo Administradores)
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Usuarios usuario)
        {
            if (!EsAdministrador()) return RedirectToAction("Index");

 
            // 🔹 Verificar si el correo ya está registrado
            if (_context.Usuarios.Any(u => u.Correo == usuario.Correo))
            {
                ViewBag.Error = "❌ Este correo ya está registrado.";
                ViewBag.Roles = _context.Roles
                    .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Nombre })
                    .ToList();
                return View(usuario);
            }

            // 🔹 Verificar si la contraseña está vacía
            if (string.IsNullOrEmpty(usuario.Contraseña))
            {
                ViewBag.Error = "❌ La contraseña no puede estar vacía.";
                ViewBag.Roles = _context.Roles
                    .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Nombre })
                    .ToList();

                ViewBag.Generos = _context.Generos
                    .Select(g => new SelectListItem { Value = g.ID.ToString(), Text = g.Nombre }) // ✅ Asegurar que se carga bien
                    .ToList();

                return View(usuario);
            }

            // 🔹 Encriptar la contraseña antes de guardarla
            usuario.Contraseña = HashPassword(usuario.Contraseña);
            usuario.Image = "/uploads/default.png"; // Imagen por defecto si no se sube una

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }



        // 📌 4. MOSTRAR FORMULARIO DE EDICIÓN (Solo Administradores)
        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id)
        {
            if (!EsAdministrador()) return RedirectToAction("Index");

            var usuario = _context.Usuarios.Find(id);
            if (usuario == null) return NotFound();

            ViewBag.Roles = _context.Roles
                .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Nombre })
                .ToList();

            return View(usuario);
        }

        // 📌 5. PROCESAR EDICIÓN (Solo Administradores)
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Usuarios usuario)
        {
            if (!EsAdministrador()) return RedirectToAction("Index");

            var usuarioExistente = _context.Usuarios.Find(usuario.ID);
            if (usuarioExistente == null) return NotFound();

            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.ApellidoPaterno = usuario.ApellidoPaterno;
            usuarioExistente.ApellidoMaterno = usuario.ApellidoMaterno;
            usuarioExistente.Usuario = usuario.Usuario;
            usuarioExistente.Correo = usuario.Correo;

            // 🔹 Solo actualizar la contraseña si el usuario ingresó una nueva
            if (!string.IsNullOrEmpty(usuario.Contraseña))
            {
                usuarioExistente.Contraseña = HashPassword(usuario.Contraseña);
            }

            // 🔹 Evitar que un administrador se cambie de rol por error
            var rolActual = HttpContext.Session.GetInt32("RolId");
            if (usuario.IDRol != usuarioExistente.IDRol && usuarioExistente.ID != HttpContext.Session.GetInt32("UsuarioId"))
            {
                usuarioExistente.IDRol = usuario.IDRol;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // 📌 6. MOSTRAR FORMULARIO DE ELIMINACIÓN (Solo Administradores)
        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            if (!EsAdministrador()) return RedirectToAction("Index");

            var usuario = _context.Usuarios.Find(id);
            if (usuario == null) return NotFound();

            return View(usuario);
        }

        // 📌 7. PROCESAR ELIMINACIÓN (Solo Administradores)
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!EsAdministrador()) return RedirectToAction("Index");

            var usuario = _context.Usuarios.Find(id);
            if (usuario == null) return NotFound();

            // 🔹 Evitar que un administrador se elimine a sí mismo
            if (usuario.ID == HttpContext.Session.GetInt32("UsuarioId"))
            {
                ViewBag.Error = "❌ No puedes eliminar tu propia cuenta.";
                return View(usuario);
            }

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ✅ Método para encriptar contraseñas
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
