using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoISNuevo.Models
{
    public class Cita
    {
        [Key]
        public string IdCita { get; set; } // 🔹 ID de la cita (6 caracteres)

        [Required]
        public int UsuarioId { get; set; } // 🔹 Paciente

        public int? DoctorId { get; set; } // 🔹 Doctor asignado (puede ser NULL al inicio)

        [Required]
        public DateTime FechaHora { get; set; } // 🔹 Fecha de la cita

        [Required]
        [StringLength(255)]
        public string Motivo { get; set; } // 🔹 Motivo de la cita

        [StringLength(50)]
        public string Estado { get; set; } = "Pendiente"; // 🔹 Estado de la cita

        // Relaciones con otras tablas
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Usuario Doctor { get; set; } // 🔹 Relación con el doctor
    }
}
