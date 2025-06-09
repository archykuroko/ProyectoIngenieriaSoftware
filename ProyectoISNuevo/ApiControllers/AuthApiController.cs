using Microsoft.AspNetCore.Mvc;
using ProyectoISNuevo.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ProyectoISNuevo.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.UsuarioOCorreo) || string.IsNullOrEmpty(request.Contraseña))
                return BadRequest("Usuario/Correo y contraseña obligatorios.");

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Usuario1 == request.UsuarioOCorreo || u.Correo == request.UsuarioOCorreo);

            if (usuario == null || usuario.Contraseña != HashPassword(request.Contraseña))
                return Unauthorized("Usuario/Correo o contraseña incorrectos.");

            var response = new LoginResponse
            {
                Id = usuario.Id,
                Usuario = usuario.Usuario1,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                RolId = usuario.Idrol ?? 1,
                Image = usuario.Image
            };

            return Ok(response);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] Usuario usuario)
        {
            if (_context.Usuarios.Any(u => u.Usuario1 == usuario.Usuario1))
                return Conflict("Usuario ya en uso.");

            if (_context.Usuarios.Any(u => u.Correo == usuario.Correo))
                return Conflict("Correo ya registrado.");

            usuario.Contraseña = HashPassword(usuario.Contraseña);
            usuario.Idrol = 1;
            usuario.Genero = 3;
            usuario.Image = "/uploads/default.png";

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return Ok("Usuario registrado correctamente.");
        }

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

    public class LoginRequest
    {
        public string UsuarioOCorreo { get; set; }
        public string Contraseña { get; set; }
    }

    public class LoginResponse
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public int RolId { get; set; }
        public string? Image { get; internal set; }
    }
}
