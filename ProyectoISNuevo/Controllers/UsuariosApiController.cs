using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoISNuevo.Models;
using System;
using System.Linq;

namespace ProyectoISNuevo.Controllers
{
    [ApiController]
    [Route("api/usuarios")]

    public class UsuariosApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/usuarios
        [HttpGet]
        public IActionResult GetAll()
        {
            var usuarios = _context.Usuarios.Include(u => u.Rol).ToList();
            return Ok(usuarios);
        }

        // ✅ GET: api/usuarios/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var usuario = _context.Usuarios.Include(u => u.Rol).FirstOrDefault(u => u.Id == id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        // ✅ POST: api/usuarios
        [HttpPost]
        public IActionResult Create([FromBody] Usuario usuario)
        {
            if (_context.Usuarios.Any(u => u.Correo == usuario.Correo))
                return Conflict("Este correo ya está registrado.");

            usuario.Contraseña = HashPassword(usuario.Contraseña);
            usuario.Image = "/uploads/default.png";

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
        }

        // ✅ PUT: api/usuarios/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Usuario usuario)
        {
            var usuarioExistente = _context.Usuarios.Find(id);
            if (usuarioExistente == null) return NotFound();

            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.ApellidoPaterno = usuario.ApellidoPaterno;
            usuarioExistente.ApellidoMaterno = usuario.ApellidoMaterno;
            usuarioExistente.Usuario1 = usuario.Usuario1;
            usuarioExistente.Correo = usuario.Correo;
            usuarioExistente.Idrol = usuario.Idrol;

            if (!string.IsNullOrWhiteSpace(usuario.Contraseña))
                usuarioExistente.Contraseña = HashPassword(usuario.Contraseña);

            _context.SaveChanges();
            return Ok("Usuario actualizado.");
        }

        // ✅ DELETE: api/usuarios/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null) return NotFound();

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
            return Ok("Usuario eliminado.");
        }

        // 🔐 Método privado para encriptar contraseñas
        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("pong from usuarios");
        }


    }
}
