using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using MoneyManager_DesktopApp.Models.ViewModels;
using MoneyManager_DesktopApp.Services;
using Newtonsoft.Json;
using Splat;
using Tmds.DBus;

namespace MoneyManager_DesktopApp.ViewModels;

public class AddWindowViewModel : INotifyPropertyChanged
{
    private RecordVM _addRecord;
    private FormsInfo _formsInfo = Locator.Current.GetService<FormsInfo>();


    public RecordVM AddRecord
    {
        get => _addRecord;
        set => _addRecord = value;
    }
    
    public ObservableCollection<CategoryVM> Categories { get; set; } = new();
    public CategoryVM SelCat { get; set; }

    public DateTimeOffset TransactionDatePicker
    {
        get => AddRecord.TransactionDate;
        set => AddRecord.TransactionDate = value.DateTime;
    }

    public AddWindowViewModel()
    {
        AddRecord = new RecordVM();
        AddRecord.TransactionDate = DateTime.Today;
        GetCategories();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    public async Task GetCategories()
    {
        HttpClientService http = new();
        var httpResponse = await http.GetListOfItems(@"https://moneymanager.hostingasp.pl/api/category");
        var recJson = await httpResponse.Content.ReadAsStringAsync();
        Categories = JsonConvert.DeserializeObject<ObservableCollection<CategoryVM>>(recJson) ?? new();
        SelCat = Categories.First();
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Categories)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelCat)));
    }

    public async Task CreateRecord()
    {
        AddRecord.CategoryId = SelCat.Id;
        HttpClientService http = new();
        var httpResponse = await http.CreateItem(AddRecord,@"https://moneymanager.hostingasp.pl/api/tracker");
        var res = httpResponse.StatusCode;

        if (httpResponse.StatusCode == HttpStatusCode.Created)
        {
            _formsInfo.RecordWindowIsOpen = false;
        }
    }
    
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