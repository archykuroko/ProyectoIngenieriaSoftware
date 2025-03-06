using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ProyectoIS.Models
{
    [Table("Roles")]
    public class Roles
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        // 🔹 Relación con los usuarios
        public virtual ICollection<Usuarios> Usuarios { get; set; }
    }
}
