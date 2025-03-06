using System.ComponentModel.DataAnnotations;

namespace ProyectoIS.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El usuario o correo es obligatorio.")]
        public string UsuarioOCorreo { get; set; } // 🔹 Un solo campo para Usuario o Correo

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; }
    }
}
