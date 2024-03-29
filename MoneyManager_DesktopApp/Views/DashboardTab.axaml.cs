using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MoneyManager_DesktopApp.ViewModels;
using MoneyManager_DesktopApp.Models.Entities;

namespace MoneyManager_DesktopApp.Views;

public partial class Dashboard : UserControl
{
    public Dashboard()
    {
        InitializeComponent();
        DataContext = new DashboardTabViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
}