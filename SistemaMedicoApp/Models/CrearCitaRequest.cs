using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMedicoApp.Models
{
    public class CrearCitaRequest
    {
        public int UsuarioId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; }
    }

}
