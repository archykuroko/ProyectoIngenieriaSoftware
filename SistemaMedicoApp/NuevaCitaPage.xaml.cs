using SistemaMedicoApp.Models;
using System.Net.Http.Json;

namespace SistemaMedicoApp;

public partial class NuevaCitaPage : ContentPage
{
    private readonly int _usuarioId;
    private readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://localhost:5001")
    };

    public NuevaCitaPage(int usuarioId)
    {
        InitializeComponent();
        _usuarioId = usuarioId;

        fechaPicker.MinimumDate = DateTime.Today;
    }

    private async void OnAgendarClicked(object sender, EventArgs e)
    {
        var fecha = fechaPicker.Date;
        var hora = horaPicker.Time;
        var fechaHora = fecha + hora;

        if (fechaHora < DateTime.Now)
        {
            await DisplayAlert("Error", "No puedes seleccionar una fecha y hora pasada.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(motivoEditor.Text))
        {
            await DisplayAlert("Error", "Debes ingresar un motivo.", "OK");
            return;
        }

        var nuevaCita = new CrearCitaRequest
        {
            UsuarioId = _usuarioId,
            FechaHora = fechaHora,
            Motivo = motivoEditor.Text
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/pruebacitas", nuevaCita);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Cita agendada correctamente.", "OK");
                await Navigation.PopAsync(); // Vuelve a la pantalla anterior
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", $"No se pudo agendar la cita.\n{error}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
