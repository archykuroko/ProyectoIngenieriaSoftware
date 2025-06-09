using SistemaMedicoApp.Models;
using Newtonsoft.Json;

namespace SistemaMedicoApp;

public partial class CitasPage : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://localhost:5001")
    };

    private readonly int _usuarioId;

    public CitasPage(int usuarioId)
    {
        InitializeComponent();
        _usuarioId = usuarioId;
        CargarCitas();
    }

    private async void CargarCitas()
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/pruebacitas/usuario/{_usuarioId}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var citas = JsonConvert.DeserializeObject<List<CitaResponse>>(json);
                citasListView.ItemsSource = citas;
            }
            else
            {
                await DisplayAlert("Error", "No se pudieron cargar las citas.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
