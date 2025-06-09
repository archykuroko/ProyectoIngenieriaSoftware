using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoISNuevo.Models;
using System;
using System.Linq;

namespace ProyectoISNuevo.Controllers
{
    [ApiController]
    [Route("api/pruebacitas")]

    public class CitasApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CitasApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔹 GET: api/citas
        [HttpGet]
        public IActionResult GetAll()
        {
            var citas = _context.Citas
                .Include(c => c.Usuario)
                .Include(c => c.Doctor)
                .ToList();

            return Ok(citas);
        }

        // 🔹 GET: api/citas/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public IActionResult GetByUsuario(int usuarioId)
        {
            var citas = _context.Citas
                .Where(c => c.UsuarioId == usuarioId)
                .Include(c => c.Doctor)
                .ToList();

            return Ok(citas);
        }

        // 🔹 GET: api/citas/buscar/ABC123
        [HttpGet("buscar/{codigo}")]
        public IActionResult BuscarPorCodigo(string codigo)
        {
            var cita = _context.Citas
                .Include(c => c.Usuario)
                .Include(c => c.Doctor)
                .FirstOrDefault(c => c.IdCita == codigo);

            if (cita == null)
                return NotFound("Cita no encontrada.");

            return Ok(cita);
        }

        // 🔹 POST: api/citas
        [HttpPost]
        public IActionResult CrearCita([FromBody] CrearCitaRequest model)
        {
            if (!_context.Usuarios.Any(u => u.Id == model.UsuarioId))
                return NotFound("Usuario no encontrado.");

            if (model.FechaHora < DateTime.Now)
                return BadRequest("No puedes seleccionar una fecha pasada.");

            var cita = new Cita
            {
                IdCita = GenerarCodigoCita(),
                UsuarioId = model.UsuarioId,
                FechaHora = model.FechaHora,
                Motivo = model.Motivo,
                Estado = "Pendiente"
            };

            _context.Citas.Add(cita);
            _context.SaveChanges();

            return Ok(new { mensaje = "Cita creada", id = cita.IdCita });
        }

        // 🔹 PUT: api/citas/ABC123/asignar
        [HttpPut("{idCita}/asignar")]
        public IActionResult AsignarDoctor(string idCita, [FromBody] AsignarDoctorRequest model)
        {
            var cita = _context.Citas.FirstOrDefault(c => c.IdCita == idCita);
            if (cita == null)
                return NotFound("Cita no encontrada.");

            var doctor = _context.Usuarios.FirstOrDefault(u => u.Id == model.DoctorId && u.Idrol == 3);
            if (doctor == null)
                return BadRequest("Doctor inválido.");

            cita.DoctorId = model.DoctorId;
            cita.Estado = "Asignada";

            _context.SaveChanges();

            return Ok("Doctor asignado correctamente.");
        }

        // 🔹 PUT: api/citas/ABC123/cancelar
        [HttpPut("{idCita}/cancelar")]
        public IActionResult CancelarCita(string idCita)
        {
            var cita = _context.Citas.FirstOrDefault(c => c.IdCita == idCita);
            if (cita == null)
                return NotFound("Cita no encontrada.");

            cita.Estado = "Cancelada";
            _context.SaveChanges();

            return Ok("Cita cancelada.");
        }

        // 🔹 Método auxiliar para generar código de cita
        private string GenerarCodigoCita()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        // ✅ PRUEBA DE FUNCIONAMIENTO DEL CONTROLADOR
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("pong");
        }
    }

    // DTOs de entrada
    public class CrearCitaRequest
    {
        public int UsuarioId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; }
    }

    public class AsignarDoctorRequest
    {
        public int DoctorId { get; set; }
    }
}
