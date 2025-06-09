using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMedicoApp.Models
{
    public class CitaResponse
    {
        public string IdCita { get; set; }
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; }
        public DoctorInfo? Doctor { get; set; }

        public class DoctorInfo
        {
            public int Id { get; set; }
            public string? Nombre { get; set; }
        }
    }
}
