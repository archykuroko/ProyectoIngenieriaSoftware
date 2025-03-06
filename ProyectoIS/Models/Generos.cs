using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoIS.Models
{
    [Table("Generos")]
    public class Generos
    {
        [Key]
        public int ID { get; set; } // 1 para Masculino, 2 para Femenino

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } // Ejemplo: "Masculino" o "Femenino"
    }
}
