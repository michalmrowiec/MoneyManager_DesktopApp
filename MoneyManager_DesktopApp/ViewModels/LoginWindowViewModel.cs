using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Splat;

namespace MoneyManager_DesktopApp.ViewModels;

public class LoginWindowViewModel : INotifyPropertyChanged
{
    private string _login;
    private string _password;
    public string Status { get; set; } = "Wait";
    public string Login
    {
        get { return _login;}
        set
        {
            _login = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Login)));
        }
    }
    public string Password
    {
        get { return _password;}
        set
        {
            _password = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
        }
    }

    public void TestBtn()
    {
        Status = "Clicked";
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
    }
    public async void LoginButton()
    {
        Status = "Loging";
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));

        var http = new HttpClient();
        var url = @"https://moneymanager.hostingasp.pl/api/account/login";
        try
        {
            var result = await http.PostAsJsonAsync(url, new {Email = Login, Password = Password});
            Status = result.StatusCode.ToString();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
            var toaster = Locator.Current.GetService<JwtTokenService>();
            toaster.Token = result.StatusCode.ToString();
            // if (result.StatusCode == HttpStatusCode.OK)
            // {
            //     var toaster = Locator.Current.GetService<JwtTokenService>();
            //     toaster.Token = result.ToString();
            // }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Status = e.Message;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
            throw;
        }


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