using SistemaMedicoApp.Models;
using Microsoft.Maui.Storage;

namespace SistemaMedicoApp;

public partial class PerfilPage : ContentPage
{
    // Campo privado para almacenar al usuario actual
    private readonly UsuarioLoginResponse _usuario;

    public PerfilPage(UsuarioLoginResponse usuario)
    {
        InitializeComponent();
        _usuario = usuario; // ← Guarda el usuario para otros métodos

        usuarioLabel.Text = usuario.Usuario;
        nombreLabel.Text = usuario.Nombre;
        correoLabel.Text = usuario.Correo;
        rolLabel.Text = ObtenerRol(usuario.RolId);

        string baseUrl = "https://localhost:5001";

        if (!string.IsNullOrWhiteSpace(usuario.Imagen))
        {
            fotoPerfil.Source = $"{baseUrl}{usuario.Imagen}";
        }
        else
        {
            fotoPerfil.Source = "default_profile.png";
        }

        // Mostrar botón solo a administradores
        if (usuario.RolId == 2)
        {
            adminButton.IsVisible = true;
        }


    }

    private string ObtenerRol(int rolId) => rolId switch
    {
        1 => "Usuario",
        2 => "Administrador",
        3 => "Doctor",
        _ => "Desconocido"
    };

    private async void OnCerrarSesionClicked(object sender, EventArgs e)
    {
        Preferences.Clear(); // Limpia los datos guardados localmente
        await Navigation.PopToRootAsync(); // Vuelve a la página inicial (Login)
    }

    private async void OnVerCitasClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CitasPage(_usuario.Id)); // Navega a la página de citas
    }

    private async void OnNuevaCitaClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NuevaCitaPage(_usuario.Id));
    }

    private async void OnVerUsuariosClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UsuariosPage());
    }

}
