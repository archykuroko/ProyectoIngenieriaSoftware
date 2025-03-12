using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoISNuevo.Models
{
    [Table("Citas")]
    public class Cita
    {
        [Key]
        [StringLength(6)]
        public string IdCita { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime FechaHora { get; set; }

        [Required]
        [StringLength(255)]
        public string Motivo { get; set; }

        [StringLength(50)]
        public string Estado { get; set; } = "Pendiente";

        // 🔹 Nueva columna para almacenar el ID del Doctor
        [Required]
        public int DoctorId { get; set; }

        // 🔹 Relaciones con otras tablas
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Usuario Doctor { get; set; }
    }
}
