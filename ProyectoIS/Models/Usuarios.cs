using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoIS.Models
{
    [Table("Usuarios")]
    public class Usuarios
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Rol")]
        public int IDRol { get; set; }

        [Required(ErrorMessage = "El campo Usuario es obligatorio.")]
        [StringLength(100)]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(100)]
        public string ApellidoPaterno { get; set; }

        [StringLength(100)]
        public string ApellidoMaterno { get; set; }

        [ForeignKey("Generos")]
        public int Genero { get; set; } // ✅ La clave foránea debe ser un INT, no un objeto

        [Required(ErrorMessage = "El campo Correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo es inválido.")]
        [StringLength(100)]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [StringLength(255)]
        public string Contraseña { get; set; }

        // 🔹 Relación con la tabla Roles
        public virtual Roles Rol { get; set; }

        // 🔹 Relación con la tabla Generos
        public virtual Generos Generos { get; set; }

        // 🔹 Foto de perfil
        public string Image { get; set; } = "/uploads/default.png";
    }
}
