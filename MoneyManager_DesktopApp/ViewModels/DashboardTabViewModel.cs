using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using MoneyManager_DesktopApp.Models.Entities;
using MoneyManager_DesktopApp.Models.ViewModels;
using MoneyManager_DesktopApp.Services;
using Newtonsoft.Json;
using Splat;

namespace MoneyManager_DesktopApp.ViewModels;

public class DashboardTabViewModel : ViewModelBase, INotifyPropertyChanged //ObservableObject
{
    public List<RecordVM> TempRecords { get; set; }
    public ObservableCollection<RecordVM> Records { get; set; }

    private ObservableCollection<int> _yearsToSelect;
    private Month _selMon;

    public ObservableCollection<int> YearsToSelect
    {
        get { return _yearsToSelect; }
        set
        {
            _yearsToSelect = value;

            SelectedYear = _yearsToSelect.First();

            var monthNo = Records
                .Where(x => x.TransactionDate.Year == SelectedYear)
                .Select(x => x.TransactionDate.Month).Distinct().ToList();

            List<Month> m = new();
            foreach (var month in monthNo)
            {
                m.Add(new Month() { Number = month, Name = new DateTime(1000, month, 1).ToString("MMMM") });
            }

            MonthsToSelect = new ObservableCollection<Month>(m.OrderByDescending(x => x.Number));


            SelectedMonth = MonthsToSelect.First();
            Records = new ObservableCollection<RecordVM>(
                TempRecords.Where(x => x.TransactionDate.Year == SelectedYear &&
                                       x.TransactionDate.Month == SelectedMonth.Number));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(YearsToSelect)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MonthsToSelect)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedYear)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMonth)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Records)));
        }
    }

    public ObservableCollection<Month> MonthsToSelect { get; set; }
    public int SelectedYear { get; set; }

    public Month SelectedMonth
    {
        get { return _selMon; }
        set
        {
            _selMon = value;
            Records = new ObservableCollection<RecordVM>(
                TempRecords.Where(x => x.TransactionDate.Year == SelectedYear &&
                                       x.TransactionDate.Month == SelectedMonth.Number));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Records)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public static int GetMonthNumberFromName(string monthName)
    {
        var dateTimeFormatInfo = new DateTimeFormatInfo();
        return dateTimeFormatInfo.MonthNames.ToList()
            .FindIndex(m => m.Equals(monthName, StringComparison.OrdinalIgnoreCase)) + 1;
    }

    public async void GetAllRecords()
    {
        //var http = new HttpClient();
        var uri = @"https://moneymanager.hostingasp.pl/api/tracker";
        try
        {
            var tokenService = Locator.Current.GetService<JwtTokenService>();

            using var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var token = tokenService.Token;
            request.Headers.Add("X-Api-Key", ConfigurationManager.AppSettings["X-Api-Key"]);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await Locator.Current.GetService<IHttpClientService>().GetListOfItems(uri);
            var recJson = await result.Content.ReadAsStringAsync();
            Records = JsonConvert.DeserializeObject<ObservableCollection<RecordVM>>(recJson) ?? new();
            TempRecords = JsonConvert.DeserializeObject<List<RecordVM>>(recJson) ?? new();

            YearsToSelect =
                new ObservableCollection<int>(Records.Select(x => x.TransactionDate.Year).Distinct().OrderDescending());

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

    public class Month
    {
        public int Number { get; set; }
        public string Name { get; set; }
    }
}