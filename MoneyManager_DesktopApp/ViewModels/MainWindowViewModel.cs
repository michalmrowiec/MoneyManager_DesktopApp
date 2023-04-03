using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using MoneyManager_DesktopApp.Views;

namespace MoneyManager_DesktopApp.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public object MainViewContent { get; set; }
    private LoginWindow? _loginWindow;

    public void OpenLoginWindow()
    {
        if (_loginWindow is null)
        {
            _loginWindow = new LoginWindow();
            _loginWindow.DataContext = new LoginWindowViewModel();
            _loginWindow.Show();
        }

        //MainViewContent = new LoginTab();
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoginWindow)));
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}