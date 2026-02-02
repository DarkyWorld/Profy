using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace Profy.Client.Services;
public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly string _serverUrl;

    public AuthService(HttpClient httpClient, string serverUrl)
    {
        _httpClient = httpClient;
        _serverUrl = serverUrl;
    }

    // Метод входа
    public async Task<bool> EnterAsync(UserEnterData userEnterData)
    {
        try
        {
            var json = JsonSerializer.Serialize(userEnterData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_serverUrl}/api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
        catch 
        {
            return false;
        }
    }

    // Метод регистрации
    public async Task<bool> RegistrationAsync(UserData userData, UserEnterData userEnterData)
    {
        try
        {

            var json = JsonSerializer.Serialize(new
            {
                userData,
                userEnterData
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_serverUrl}/auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                MessageBox.Show("Регистрация успешна");
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка регистрации: {response.StatusCode} - {errorContent}");
                return false;
            }
        }
        catch 
        {
            return false;
        }
    }
}