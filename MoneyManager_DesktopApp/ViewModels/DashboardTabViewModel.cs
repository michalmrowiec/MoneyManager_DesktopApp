using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive;
using CommunityToolkit.Mvvm.ComponentModel;
using MoneyManager_DesktopApp.Models.ViewModels;
using MoneyManager_DesktopApp.Services;
using Newtonsoft.Json;
using Splat;


namespace MoneyManager_DesktopApp.ViewModels;

public class DashboardTabViewModel : ViewModelBase, INotifyPropertyChanged //ObservableObject
{
    //public List<RecordVM> Records { get; set; }
    public ObservableCollection<RecordVM> Records { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;

    public async void GetAllRecords()
    {
        var http = new HttpClient();
        var uri = @"https://moneymanager.hostingasp.pl/api/tracker";
        try
        {
            var toaster = Locator.Current.GetService<JwtTokenService>();
            
            using var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var token = toaster.Token;
            request.Headers.Add("X-Api-Key", ConfigurationManager.AppSettings["X-Api-Key"]);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //var result = await http.SendAsync(request);

            var result = await Locator.Current.GetService<IHttpClientService>().GetListOfItems(uri);
            var recJson = await result.Content.ReadAsStringAsync();
            Records = JsonConvert.DeserializeObject<ObservableCollection<RecordVM>>(recJson) ?? new();
            
            foreach (var recordVm in Records)
            {
                recordVm.CategoryName = recordVm.Category?.Name;
            }
            
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