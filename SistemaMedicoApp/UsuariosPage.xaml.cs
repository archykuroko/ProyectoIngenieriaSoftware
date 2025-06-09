using SistemaMedicoApp.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace SistemaMedicoApp;

public partial class UsuariosPage : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://localhost:5001")
    };

    public UsuariosPage()
    {
        InitializeComponent();
        CargarUsuarios();
    }

    private async void CargarUsuarios()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/usuarios");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var usuarios = JsonConvert.DeserializeObject<List<UsuarioCrud>>(json);
                usuariosListView.ItemsSource = usuarios;
            }
            else
            {
                await DisplayAlert("Error", "No se pudo cargar la lista.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnNuevoUsuarioClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditarUsuarioPage(null));
    }

    private async void OnEditarClicked(object sender, EventArgs e)
    {
        var usuario = (sender as Button)?.CommandParameter as UsuarioCrud;
        if (usuario != null)
        {
            await Navigation.PushAsync(new EditarUsuarioPage(usuario));
        }
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        var usuario = (sender as Button)?.CommandParameter as UsuarioCrud;
        if (usuario == null) return;

        bool confirm = await DisplayAlert("Confirmar", $"¿Eliminar a {usuario.Nombre}?", "Sí", "No");
        if (!confirm) return;

        var response = await _httpClient.DeleteAsync($"/api/usuarios/{usuario.Id}");
        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("Éxito", "Usuario eliminado.", "OK");
            CargarUsuarios();
        }
        else
        {
            await DisplayAlert("Error", "No se pudo eliminar.", "OK");
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarUsuarios();
    }
}
