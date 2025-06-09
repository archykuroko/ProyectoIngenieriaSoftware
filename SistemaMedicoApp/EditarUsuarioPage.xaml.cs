using SistemaMedicoApp.Models;
using System.Net.Http.Json;

namespace SistemaMedicoApp;

public partial class EditarUsuarioPage : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://localhost:5001")
    };

    private UsuarioCrud _usuario;

    public EditarUsuarioPage(UsuarioCrud? usuario)
    {
        InitializeComponent();

        _usuario = usuario ?? new UsuarioCrud();

        if (usuario != null)
        {
            nombreEntry.Text = usuario.Nombre;
            usuarioEntry.Text = usuario.Usuario1;
            correoEntry.Text = usuario.Correo;
            rolEntry.Text = usuario.Idrol?.ToString();
        }
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        _usuario.Nombre = nombreEntry.Text;
        _usuario.Usuario1 = usuarioEntry.Text;
        _usuario.Correo = correoEntry.Text;
        _usuario.Contraseña = contraseñaEntry.Text;
        _usuario.Idrol = int.TryParse(rolEntry.Text, out var rol) ? rol : 1;

        HttpResponseMessage response;

        if (_usuario.Id > 0)
        {
            response = await _httpClient.PutAsJsonAsync($"/api/usuarios/{_usuario.Id}", _usuario);
        }
        else
        {
            response = await _httpClient.PostAsJsonAsync("/api/usuarios", _usuario);
        }

        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("Éxito", "Usuario guardado.", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            await DisplayAlert("Error", error, "OK");
        }
    }
}
