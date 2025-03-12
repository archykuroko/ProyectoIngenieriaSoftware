using System;
using System.Collections.Generic;

namespace ProyectoISNuevo.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public int? Idrol { get; set; }

    public string? Usuario1 { get; set; }

    public string? Nombre { get; set; }

    public string? ApellidoPaterno { get; set; }

    public string? ApellidoMaterno { get; set; }

    public int? Genero { get; set; }

    public string? Correo { get; set; }

    public string? Contraseña { get; set; }

    public string? Image { get; set; }

    public virtual Role? Rol { get; set; }
}
