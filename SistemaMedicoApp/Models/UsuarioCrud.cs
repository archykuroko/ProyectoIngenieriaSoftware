using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMedicoApp.Models
{
    public class UsuarioCrud
    {
        public int Id { get; set; }
        public string? Usuario1 { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? Correo { get; set; }
        public string? Contraseña { get; set; }
        public int? Idrol { get; set; }
        public string? Image { get; set; }

        public Role? Rol { get; set; } // Para mostrar el nombre del rol

        public class Role
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }
    }
}
