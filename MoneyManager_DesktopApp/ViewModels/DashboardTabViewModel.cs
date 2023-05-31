using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Avalonia.Controls;
using MoneyManager_DesktopApp.Models.ViewModels;
using MoneyManager_DesktopApp.Services;
using MoneyManager_DesktopApp.Views;
using Newtonsoft.Json;
using Splat;

namespace MoneyManager_DesktopApp.ViewModels;

public class DashboardTabViewModel : ViewModelBase, INotifyPropertyChanged //ObservableObject
{
    public string SumOfCurrentMonth { get; set; } = "";
    public List<RecordVM> TempRecords { get; set; }
    public ObservableCollection<RecordVM> Records { get; set; }

    private ObservableCollection<int> _yearsToSelect;
    public RecordVM? SelectedRecord { get; set; }
    private Month _selectedMonth;
    private int _selectedYear;
    private AddWindow? _addWindow;
    private FormsInfo _formsInfo = Locator.Current.GetService<FormsInfo>();

    public ObservableCollection<int> YearsToSelect
    {
        get { return _yearsToSelect; }
        set
        {
            _yearsToSelect = value;

            SelectedYear = _yearsToSelect.First();
        }
    }

    public ObservableCollection<Month> MonthsToSelect { get; set; }

    public int SelectedYear
    {
        get { return _selectedYear; }
        set
        {
            if (value == 0)
            {
                CalculateSumOfMonth();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(YearsToSelect)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MonthsToSelect)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedYear)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMonth)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Records)));
                return;
            }

            _selectedYear = value;

            var monthsNumbers = TempRecords
                .Where(x => x.TransactionDate.Year == _selectedYear)
                .Select(x => x.TransactionDate.Month).Distinct().ToList();

            List<Month> months = new();
            foreach (var month in monthsNumbers)
            {
                months.Add(new Month() { Number = month, Name = new DateTime(1000, month, 1).ToString("MMMM") });
            }

            MonthsToSelect = new ObservableCollection<Month>(months.OrderBy(x => x.Number));


            SelectedMonth = MonthsToSelect.First();
        }
    }

    public Month SelectedMonth
    {
        get { return _selectedMonth; }
        set
        {
            if (value == null)
            {
                CalculateSumOfMonth();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(YearsToSelect)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MonthsToSelect)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedYear)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMonth)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Records)));
                return;
            }

            _selectedMonth = value;

            Records = new ObservableCollection<RecordVM>(
                TempRecords.Where(x => x.TransactionDate.Year == SelectedYear &&
                                       x.TransactionDate.Month == _selectedMonth.Number));

            CalculateSumOfMonth();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(YearsToSelect)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MonthsToSelect)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedYear)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMonth)));
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

    public DashboardTabViewModel()
    {
        GetAllRecords();
    }

    public async void GetAllRecords()
    {
        _formsInfo.OnChange -= GetAllRecords;

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

            foreach (var recordVm in Records)
            {
                recordVm.CategoryName = recordVm.Category?.Name;
            }

            YearsToSelect =
                new ObservableCollection<int>(Records.Select(x => x.TransactionDate.Year).Distinct()
                    .OrderDescending());

            // if (TempRecords.Exists(x => x.TransactionDate.Year == SelectedYear))
            //     SelectedYear = SelectedYear;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Update()
    {
        if (SelectedRecord != null)
        {
            _addWindow = new AddWindow
            {
                DataContext = new AddWindowViewModel(SelectedRecord),
                Width = 400,
                Height = 285,
            };
            _addWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _addWindow.Show();
            _addWindow.Activate();
            _formsInfo.RecordWindowIsOpen = true;
            _formsInfo.OnChange += _addWindow.Close;
            _formsInfo.OnChange += GetAllRecords;
        }

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddWindow)));
    }

    private void CalculateSumOfMonth()
    {
        SumOfCurrentMonth = $"Suma wydatków i przychodów w wybranym miesiącu: {Records.Sum(x => x.Amount)}zł";
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SumOfCurrentMonth)));

    }

    public class Month
    {
        public int Number { get; set; }
        public string Name { get; set; }
    }
}