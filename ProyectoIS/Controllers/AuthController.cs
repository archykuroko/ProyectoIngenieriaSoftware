using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProyectoIS.Data;
using ProyectoIS.Models;

namespace ProyectoIS.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===================== REGISTRO =====================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "❌ Datos incorrectos.";
                return View(model);
            }

            // 🔹 Verificar si el usuario o correo ya están registrados
            if (_context.Usuarios.Any(u => u.Usuario == model.Usuario))
            {
                ViewBag.Message = "❌ Este usuario ya está en uso.";
                return View(model);
            }

            if (_context.Usuarios.Any(u => u.Correo == model.Correo))
            {
                ViewBag.Message = "❌ Este correo ya está registrado.";
                return View(model);
            }

            // 🔹 Crear el usuario con los datos proporcionados
            var usuario = new Usuarios
            {
                Usuario = model.Usuario,
                Nombre = model.Nombre,
                ApellidoPaterno = model.ApellidoPaterno,
                ApellidoMaterno = model.ApellidoMaterno,
                Correo = model.Correo,
                Contraseña = HashPassword(model.Contraseña),
                IDRol = 1, // Rol por defecto
                Genero = 3, // Asigna "No seleccionado" si es NULL
                Image = "/uploads/default.png" // Imagen por defecto
            };


            try
            {
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.Message = "❌ Error en la base de datos: " + innerMessage;
                return View(model);
            }
        }

        // ===================== LOGIN =====================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "❌ Usuario/Correo y contraseña son obligatorios.";
                return View();
            }

            try
            {
                if (_context.Usuarios == null)
                {
                    ViewBag.Error = "❌ Error interno: La base de datos no está conectada.";
                    return View();
                }

                // ✅ Buscar usuario por Usuario o Correo
                var usuario = _context.Usuarios
                    .FirstOrDefault(u => (u.Usuario == model.UsuarioOCorreo || u.Correo == model.UsuarioOCorreo));

                if (usuario == null)
                {
                    ViewBag.Error = "❌ Usuario o correo no encontrado.";
                    return View();
                }

                usuario.Image = string.IsNullOrEmpty(usuario.Image) ? "/uploads/default.png" : usuario.Image;

                // ✅ Verificar la contraseña
                if (!VerifyPassword(model.Contraseña, usuario.Contraseña))
                {
                    ViewBag.Error = "❌ Usuario/Correo o contraseña incorrectos.";
                    return View();
                }

                // ✅ Guardar sesión
                HttpContext.Session.SetInt32("UsuarioId", usuario.ID);
                HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
                HttpContext.Session.SetString("Correo", usuario.Correo);
                HttpContext.Session.SetInt32("RolId", usuario.IDRol);

                Console.WriteLine("✅ Sesión almacenada correctamente.");

                // ✅ Crear lista de claims para autenticación con cookies
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Email, usuario.Correo),
            new Claim(ClaimTypes.Role, usuario.IDRol == 1 ? "Usuario" : usuario.IDRol == 2 ? "Administrador" : "Doctor")
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                Console.WriteLine("✅ Usuario autenticado con cookies.");

                return usuario.IDRol == 2
                    ? RedirectToAction("Index", "Admin")
                    : RedirectToAction("Index", "Home"); // 🔹 Ajusta esto según tu proyecto
            }
            catch (Exception ex)
            {
                ViewBag.Error = "❌ Error inesperado: " + ex.Message;
                return View();
            }
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

        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            return HashPassword(inputPassword) == storedPassword;
        }

        // ===================== LOGOUT =====================
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
