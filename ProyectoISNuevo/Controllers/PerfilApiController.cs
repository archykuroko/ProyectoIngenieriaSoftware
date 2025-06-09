using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoISNuevo.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace ProyectoISNuevo.Controllers
{
    [ApiController]
    [Route("api/perfil")]
    public class PerfilApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PerfilApiController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ✅ GET: api/perfil/5
        [HttpGet("{id}")]
        public IActionResult GetPerfil(int id)
        {
            var usuario = _context.Usuarios.Include(u => u.Rol).FirstOrDefault(u => u.Id == id);
            if (usuario == null) return NotFound("Usuario no encontrado.");

            return Ok(usuario);
        }

        // ✅ PUT: api/perfil/5
        [HttpPut("{id}")]
        public IActionResult EditarPerfil(int id, [FromBody] EditarPerfilDto model)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null) return NotFound("Usuario no encontrado.");

            usuario.Nombre = model.Nombre;
            usuario.ApellidoPaterno = model.ApellidoPaterno;
            usuario.ApellidoMaterno = model.ApellidoMaterno;
            usuario.Genero = model.Genero;

            _context.SaveChanges();

            return Ok("Perfil actualizado.");
        }

        // ✅ POST: api/perfil/5/foto
        [HttpPost("{id}/foto")]
        public IActionResult SubirFoto(int id, IFormFile archivo)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null) return NotFound("Usuario no encontrado.");
            if (archivo == null || archivo.Length == 0) return BadRequest("Archivo inválido.");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var nombreArchivo = $"{id}_{Path.GetFileName(archivo.FileName)}";
            var rutaCompleta = Path.Combine(uploadsFolder, nombreArchivo);

            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                archivo.CopyTo(stream);
            }

            usuario.Image = $"/uploads/{nombreArchivo}";
            _context.SaveChanges();

            return Ok(new { mensaje = "Imagen actualizada.", ruta = usuario.Image });
        }
    }

    // 🔸 DTO para editar perfil
    public class EditarPerfilDto
    {
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int Genero { get; set; }
    }
}
