using System.ComponentModel.DataAnnotations;

namespace ProyectoIS.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "El apellido paterno es obligatorio.")]
        public string ApellidoPaterno { get; set; }

        [Required(ErrorMessage = "El apellido materno es obligatorio.")]
        public string ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Contraseña { get; set; }

        public int? Genero { get; set; } = 3; // Género por defecto "No seleccionado"
    }

}

