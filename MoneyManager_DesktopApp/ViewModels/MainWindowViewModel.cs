using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Microsoft.VisualBasic;
using MoneyManager_DesktopApp.Services;
using MoneyManager_DesktopApp.Views;
using Splat;

//using System.Configuration
//ConfiguratinManager.AppSettings["key"]

namespace MoneyManager_DesktopApp.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public object MainViewContent { get; set; }
    private LoginWindow? _loginWindow;
    private AddWindow? _addWindow;
    private FormsInfo _formsInfo = Locator.Current.GetService<FormsInfo>();

    public MainWindowViewModel()
    {
        OpenLoginWindow();
    }

    public async Task OpenLoginWindow()
    {
        await Task.Delay(10);
        // if (_loginWindow is null)
        if (true)
        {
            _loginWindow = new LoginWindow
            {
                DataContext = new LoginWindowViewModel(),
                Width = 300,
                Height = 170
            };
            _loginWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _loginWindow.Show();
            _loginWindow.Activate();
            _formsInfo.LoginWindowIsOpen = true;
            _formsInfo.OnChange += _loginWindow.Close;
        }

        var toaster = Locator.Current.GetService<JwtTokenService>();
        var s = toaster.Token;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoginWindow)));
    }

    public void OpenAddWindow()
    {
        if (true)
        {
            _addWindow = new AddWindow
            {
                DataContext = new AddWindowViewModel(),
                Width = 400,
                Height = 285
            };
            _addWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _addWindow.Show();
            _addWindow.Activate();
            _formsInfo.RecordWindowIsOpen = true;
            _formsInfo.OnChange += _addWindow.Close;
        }

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddWindow)));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void OpenDashboardView()
    {
        MainViewContent = new Dashboard();
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MainViewContent)));
    }   
    
    private void OpenCategoryView()
    {
        MainViewContent = new CategoryTab();
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MainViewContent)));
    }

    private void OpenStartTabView()
    {
        MainViewContent = new StartTab();
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MainViewContent)));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}