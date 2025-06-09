using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMedicoApp.Models;

public class UsuarioLoginResponse
{
    public int Id { get; set; }
    public string Usuario { get; set; }
    public string Nombre { get; set; }
    public string Correo { get; set; }
    public int RolId { get; set; }

    [JsonProperty("Image")]
    public string? Imagen { get; set; }
}