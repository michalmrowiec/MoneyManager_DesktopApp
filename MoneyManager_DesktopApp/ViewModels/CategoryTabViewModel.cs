using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Avalonia.Controls;
using MoneyManager_DesktopApp.Models.ViewModels;
using MoneyManager_DesktopApp.Services;
using MoneyManager_DesktopApp.Views;
using Newtonsoft.Json;
using Splat;

namespace MoneyManager_DesktopApp.ViewModels;

public class CategoryTabViewModel : ViewModelBase, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<CategoryVM> Categories { get; set; }
    private FormsInfo _formsInfo = Locator.Current.GetService<FormsInfo>();
    public AddCategoryWindow AddCategoryWindow { get; set; }

    public CategoryTabViewModel()
    {
        GetAllCategories();
    }

    public void OpenCategoryWindow()
    {
        AddCategoryWindow = new AddCategoryWindow
        {
            DataContext = new AddCategoryWindowViewModel(),
            Width = 400,
            Height = 285,
        };
        AddCategoryWindow.Show();
        AddCategoryWindow.Activate();
        _formsInfo.CategoryWindowIsOpen = true;
        _formsInfo.OnChange += AddCategoryWindow.Close;
        _formsInfo.OnChange += GetAllCategories;

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddWindow)));
    }

    public async void GetAllCategories()
    {
        var uri = @"https://moneymanager.hostingasp.pl/api/category";
        try
        {
            var tokenService = Locator.Current.GetService<JwtTokenService>();

            using var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var token = tokenService.Token;
            request.Headers.Add("X-Api-Key", ConfigurationManager.AppSettings["X-Api-Key"]);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await Locator.Current.GetService<IHttpClientService>().GetListOfItems(uri);
            var recJson = await result.Content.ReadAsStringAsync();
            Categories = JsonConvert.DeserializeObject<ObservableCollection<CategoryVM>>(recJson) ?? new();


            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Categories)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}