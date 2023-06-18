using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MoneyManager_DesktopApp.Models.ViewModels;
using MoneyManager_DesktopApp.Services;
using Splat;

namespace MoneyManager_DesktopApp.ViewModels;

public class AddCategoryWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public string Status { get; set; } = "";
    private FormsInfo _formsInfo = Locator.Current.GetService<FormsInfo>();

    public CategoryVM AddCategory { get; set; } = new();
    
    public async Task CreateCategory()
    {
        HttpResponseMessage? httpResponse = null;
        HttpClientService http = new();
        
        httpResponse = await http.CreateItem(AddCategory, @"https://moneymanager.hostingasp.pl/api/category");

        if (httpResponse?.StatusCode == HttpStatusCode.Created)
            Status = "Utworzono";
        else if (httpResponse?.StatusCode == HttpStatusCode.OK)
            Status = "Zmodyfikowano";
        else
            Status = httpResponse?.StatusCode.ToString();

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));

        await Task.Delay(500);
        _formsInfo.CategoryWindowIsOpen = false;
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