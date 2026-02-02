using Profy.Client.Services;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace Profy.Client.ViewModels;

public class EnterViewModel : ViewModelBase
{
    private UsersDataLogin usersDataLogin;
    public UsersDataLogin UserEnterData
    {
        get => usersDataLogin;
        set => SetField(ref usersDataLogin, value);
    }
    private UsersData userData;
    public UsersData UserData
    {
        get => userData;
        set => SetField(ref userData, value);
    }
    private HttpClient _httpClient;
    private string _serverUrl;
    private AuthService _authService;

    public ICommand RegistrationCommand { get; }
    public ICommand EnterCommand { get; }



    public EnterViewModel()
    {
        _httpClient = new HttpClient();
        GetServerUrl();
        _authService = new AuthService(_httpClient, _serverUrl);

        EnterCommand = new LambdaCommand(
            async(_) => EnterAsync(),
            _ => !string.IsNullOrEmpty(UserEnterData.Login)&&
            !string.IsNullOrEmpty(UserEnterData.Password)
        );
        RegistrationCommand = new LambdaCommand(async (_) => Task.Run(Registration));
    }
    private async void EnterAsync()
    {
        if (await _authService.EnterAsync(UserEnterData))
        {
            var newWindow = new MainWindow(UserEnterData);
            newWindow.Show();
            CloseWindow();
        }
        else 
        {
            MessageBox.Show("Неверный логин или пароль");
        }
    }
    private async void Registration()
    {
        if (await _authService.RegistrationAsync(UserData,UserEnterData))
        {
            var newWindow = new MainWindow(UserEnterData);
            newWindow.Show();
            CloseWindow();
        }
    }
    private void GetServerUrl()
    {
        _serverUrl = "http://localhost:5000";
        var configPath = "ServerUrl.json";
        if (System.IO.File.Exists(configPath))
        {
            try
            {
                var json = System.IO.File.ReadAllText(configPath);
                var tempConfig = System.Text.Json.JsonSerializer.Deserialize<string>(json);
                if (tempConfig != null)
                {
                    _serverUrl = tempConfig;
                }
            }
            catch { }
        }
        else
        {
            var json = System.Text.Json.JsonSerializer.Serialize(_serverUrl);
            System.IO.File.WriteAllText(configPath, json);
        }
    }
    private void CloseWindow()
    {
        var window = System.Windows.Application.Current.Windows
            .OfType<System.Windows.Window>()
            .FirstOrDefault(w => w.DataContext == this);
        window?.Close();
    }
}

