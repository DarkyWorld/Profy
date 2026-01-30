

namespace MonitoringProgram.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    public string UserLogin
    {
        get;
        set => SetField(ref field, value);
    }
    public int Password
    {
        get;
        set => SetField(ref field, value);
    }

   
    // Команды
    public ICommand AddWordCommand { get; }
    public ICommand RemoveWordCommand { get; }
    public ICommand AddProgramCommand { get; }
    public ICommand RemoveProgramCommand { get; }
    public ICommand BrowsePathCommand { get; }
    public ICommand SaveConfigCommand { get; }
    public ICommand ResetConfigCommand { get; }
    public ICommand StartMonitoringCommand { get; }
    public ICommand StopMonitoringCommand { get; }
    public ICommand ClearLogsCommand { get; }


    public MainWindowViewModel()
    {





        _keyboardHookService = new KeyboardHookService();
        _keyboardHookService.PropertyChanged += (s, e) =>
        {
            OnPropertyChanged(nameof(LastKeysPressed));
        };
        _cancellationTokenSource = new CancellationTokenSource();

        // Загрузка конфигурации
        LoadConfig();
        _reportPath = !string.IsNullOrEmpty(Config?.ReportPath)
                                            ? Config.ReportPath
                                            : Environment.CurrentDirectory;
        // Инициализация команд
        AddWordCommand = new LambdaCommand(_ => AddWord());
        RemoveWordCommand = new LambdaCommand(_ => RemoveWord());
        AddProgramCommand = new LambdaCommand(_ => AddProgram());
        RemoveProgramCommand = new LambdaCommand(_ => RemoveProgram());
        BrowsePathCommand = new LambdaCommand(_ => BrowsePath());
        SaveConfigCommand = new LambdaCommand(_ => SaveConfig());
        ResetConfigCommand = new LambdaCommand(_ => Config = new MonitoringConfig());
        StartMonitoringCommand = new LambdaCommand(_ => StartMonitoringAsync());
        StopMonitoringCommand = new LambdaCommand(_ => StopMonitoring());
        ClearLogsCommand = new LambdaCommand(_ => ClearLogs());
    }


}


