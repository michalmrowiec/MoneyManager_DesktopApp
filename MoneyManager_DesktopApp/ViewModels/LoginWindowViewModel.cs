using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using MoneyManager_DesktopApp.Models.ViewModels;
using Newtonsoft.Json;
using Splat;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using MoneyManager_DesktopApp.Services;

namespace MoneyManager_DesktopApp.ViewModels;

public class LoginWindowViewModel : INotifyPropertyChanged
{
    private string _login;
    private string _password;
    private FormsInfo _formsInfo = Locator.Current.GetService<FormsInfo>();

    public string Status { get; set; } = "Wpisz email i hasło";

    public string Login
    {
        get { return _login; }
        set
        {
            _login = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Login)));
        }
    }

    public string Password
    {
        get { return _password; }
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
        Status = "Logowanie";
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));

        HttpResponseMessage result;
        var http = new HttpClient();
        var url = @"https://moneymanager.hostingasp.pl/api/account/login";
        try
        {
            var toaster = Locator.Current.GetService<JwtTokenService>();

            http.DefaultRequestHeaders.Add("X-Api-Key", ConfigurationManager.AppSettings["X-Api-Key"]);
            result = await http.PostAsJsonAsync(url, new { Email = Login, Password = Password });
            UserTokenVM userToken =
                JsonConvert.DeserializeObject<UserTokenVM>(await result.Content.ReadAsStringAsync()) ?? new();
            toaster.Token = userToken.Token;

            if (result.StatusCode == HttpStatusCode.OK)
            {
                Status = "Zalogowano";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
                await Task.Delay(1000);
                _formsInfo.LoginWindowIsOpen = false;
            }
            else if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                Status = "Błędny email lub hasło";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
            }

        }
        catch (Exception e)
        {
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