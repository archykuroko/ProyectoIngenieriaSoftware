using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ProyectoISNuevo.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;


namespace ProyectoISNuevo.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===================== LOGIN =====================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string usuarioOCorreo, string contraseña)
        {
            if (string.IsNullOrEmpty(usuarioOCorreo) || string.IsNullOrEmpty(contraseña))
            {
                ViewBag.Error = "⚠️ Usuario/Correo y contraseña son obligatorios.";
                return View();
            }

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Usuario1 == usuarioOCorreo || u.Correo == usuarioOCorreo);

            if (usuario == null || usuario.Contraseña != HashPassword(contraseña))
            {
                ViewBag.Error = "❌ Usuario/Correo o contraseña incorrectos.";
                return View();
            }

            // ✅ Guardar sesión
            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("Usuario", usuario.Usuario1);
            HttpContext.Session.SetString("Nombre", usuario.Nombre);
            HttpContext.Session.SetString("Correo", usuario.Correo);
            HttpContext.Session.SetInt32("RolId", usuario.Idrol.GetValueOrDefault(1)); // Si es null, asigna 1 (Usuario)


            return RedirectToAction("Index", "Dashboard");
        }

        // ===================== REGISTRO =====================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "❌ Datos incorrectos.";
                return View(usuario);
            }

            // 🔹 Verificar si el usuario o correo ya están registrados
            if (_context.Usuarios.Any(u => u.Usuario1 == usuario.Usuario1))
            {
                ViewBag.Message = "❌ Este usuario ya está en uso.";
                return View(usuario);
            }

            if (_context.Usuarios.Any(u => u.Correo == usuario.Correo))
            {
                ViewBag.Message = "❌ Este correo ya está registrado.";
                return View(usuario);
            }

            // 🔹 Crear el usuario con valores predeterminados
            usuario.Contraseña = HashPassword(usuario.Contraseña);
            usuario.Idrol = 1; // Rol por defecto (Usuario)
            usuario.Genero = 3; // Género "No seleccionado"
            usuario.Image = "/uploads/default.png"; // Imagen por defecto

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            // ✅ Agregar mensaje de éxito
            ViewBag.Success = "✅ Usuario registrado correctamente.";

            return RedirectToAction("Login");
        }


        // ===================== LOGOUT =====================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ===================== HASH CONTRASEÑA =====================
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }



    }
}
