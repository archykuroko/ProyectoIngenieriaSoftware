namespace ProyectoIS.Models.ViewModels
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }

        public string Usuario { get; set; } // Nuevo campo Usuario

        public string Nombre { get; set; }

        public string ApellidoPaterno { get; set; } // Nuevo campo ApellidoPaterno

        public string ApellidoMaterno { get; set; } // Nuevo campo ApellidoMaterno

        public string Correo { get; set; }

        // 🔹 Nombre del Rol en lugar del ID
        public string RolNombre { get; set; }

        // 🔹 Nombre del Género en lugar del ID
        public string GeneroNombre { get; set; }

        // 🔹 Imagen de perfil para mostrarla correctamente
        public string Image { get; set; }
    }
}
