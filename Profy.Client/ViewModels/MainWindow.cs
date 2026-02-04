using System.Collections.ObjectModel;
using System.Data;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Input;

namespace Profy.Client.ViewModels;

public class MainWindow : ViewModelBase
{
    private ObservableCollection<UsersData> Users { get; set; } = [];
    public ObservableCollection<UsersData> FindongUsers { get; set; } = [];
    public UsersData CurrentUser { get; set; }
    private string _serverUrl;

    private string _searchLine;
    public string SearchLine
    {
        get => _searchLine;
        set => SetField(ref _searchLine, value);
    }
    public ICommand SearchCommand {  get;}
    public MainWindow(UsersData userData, string serverUrl)
    {
        CurrentUser = userData;
        _serverUrl = serverUrl;
        SearchCommand = new LambdaCommand(
            _ => Search(),
            _ => string.IsNullOrEmpty(SearchLine));

        LoadDataUser();
    }
    private void Search()
    {
        SearchLine = string.Empty;
        IEnumerable<UsersData>? FiltredUsers = [];

        FiltredUsers = Users.Where(u =>
            u.LastName.Contains(SearchLine!, StringComparison.OrdinalIgnoreCase)||
            u.FirstName.Contains(SearchLine!, StringComparison.OrdinalIgnoreCase) ||
            u.MiddleName.Contains(SearchLine!, StringComparison.OrdinalIgnoreCase) ||
            u.Experience.Contains(SearchLine!, StringComparison.OrdinalIgnoreCase) ||
            u.Specialization.Contains(SearchLine!, StringComparison.OrdinalIgnoreCase));


        FindongUsers.Clear();
        foreach (var book in FiltredUsers)
        {
            FindongUsers.Add(book);
        }
    }
    private async Task LoadDataUser()
    {
        try
        {
            HttpClient httpClient = new HttpClient();
            var usersFromServer = await httpClient.GetFromJsonAsync<List<DataUser>>($"{_serverUrl}/users");

            Users.Clear();
            foreach (var user in usersFromServer)
            {
                Users.Add(user);
            }
        }
        catch { }
    }
}
