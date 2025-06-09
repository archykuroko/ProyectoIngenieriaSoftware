using Newtonsoft.Json;
using SistemaMedicoApp.Models;
using System.Net.Http.Json;
using System.Text.Json.Serialization;



namespace SistemaMedicoApp;

public partial class LoginPage : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://localhost:5001") // Este para Windows Machine
        //BaseAddress = new Uri("https://10.0.2.2:5001") /* Esto para android emulador*/
    };

    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var loginData = new
        {
            usuarioOCorreo = usuarioEntry.Text,
            contraseña = contraseñaEntry.Text
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/authapi/login", loginData);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var usuario = JsonConvert.DeserializeObject<UsuarioLoginResponse>(json);

                await DisplayAlert("Bienvenido", $"Hola {usuario.Nombre}", "Continuar");

                await Shell.Current.Navigation.PushAsync(new PerfilPage(usuario));
            }
            else
            {
                await DisplayAlert("Error", "Usuario o contraseña inválidos.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnRegistroClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegistroPage());
    }


}