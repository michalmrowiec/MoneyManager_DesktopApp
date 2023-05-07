using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
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

    public void OpenLoginWindow()
    {
        // if (_loginWindow is null)
        if (true)
        {
            _loginWindow = new LoginWindow
            {
                DataContext = new LoginWindowViewModel(),
                Width = 300,
                Height = 150
            };
            _loginWindow.Show();
        }
        
        var toaster = Locator.Current.GetService<JwtTokenService>();
        var s = toaster.Token;
        
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoginWindow)));
    }
    
    public void OpenAddWindow()
    {
        if (_addWindow is null)
        {
            _addWindow = new AddWindow
            {
                DataContext = new AddWindowViewModel(),
                Width = 300,
                Height = 150
            };
            _addWindow.Show();
        }

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoginWindow)));
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