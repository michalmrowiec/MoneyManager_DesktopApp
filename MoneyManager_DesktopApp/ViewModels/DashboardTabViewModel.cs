using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MoneyManager_DesktopApp.Models.ViewModels;
using MoneyManager_DesktopApp.Views;
using Newtonsoft.Json;
using Splat;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Configuration;

namespace MoneyManager_DesktopApp.ViewModels;

public class DashboardTabViewModel : INotifyPropertyChanged
{
    //public List<RecordVM> Records { get; set; }
    public ObservableCollection<RecordVM> Records { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;
    public int Status { get; set; } = 65441;
    public void TestBtn()
    {
        Status++;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Records)));

    }

    public async void GetAllRecords()
    {
        var http = new HttpClient();
        var uri = @"https://moneymanager.hostingasp.pl/api/tracker";
        try
        {
            var toaster = Locator.Current.GetService<JwtTokenService>();
            
            using var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var token = toaster.Token;
            //request.Headers.Add(toaster.ApiKey().Item1, toaster.ApiKey().Item2);
            request.Headers.Add("X-Api-Key", ConfigurationManager.AppSettings["X-Api-Key"]);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await http.SendAsync(request);

            var recJson = await result.Content.ReadAsStringAsync();
            Records = JsonConvert.DeserializeObject<ObservableCollection<RecordVM>>(recJson) ?? new ObservableCollection<RecordVM>();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Records)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Records)));
    }
}