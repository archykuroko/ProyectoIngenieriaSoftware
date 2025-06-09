using SistemaMedicoApp.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace SistemaMedicoApp;

public partial class RegistroPage : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://localhost:5001") // Cambia si usas Android
    };

    public RegistroPage()
    {
        InitializeComponent();
    }

    private async void OnRegistroClicked(object sender, EventArgs e)
    {
        var nuevoUsuario = new UsuarioRegistro
        {
            Nombre = nombreEntry.Text,
            Usuario1 = usuarioEntry.Text,
            Correo = correoEntry.Text,
            Contraseña = contraseñaEntry.Text
        };

        if (string.IsNullOrWhiteSpace(nuevoUsuario.Usuario1) ||
            string.IsNullOrWhiteSpace(nuevoUsuario.Contraseña) ||
            string.IsNullOrWhiteSpace(nuevoUsuario.Nombre) ||
            string.IsNullOrWhiteSpace(nuevoUsuario.Correo))
        {
            await DisplayAlert("Error", "Completa todos los campos.", "OK");
            return;
        }

        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/authapi/register", nuevoUsuario);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Usuario registrado correctamente.", "Iniciar sesión");
                await Navigation.PopAsync(); // Regresa a Login
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", error, "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnVolverLoginClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(); // Vuelve a la página anterior (LoginPage)
    }


}
