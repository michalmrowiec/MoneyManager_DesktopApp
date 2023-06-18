using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MoneyManager_DesktopApp.ViewModels;
using MoneyManager_DesktopApp.Models.Entities;

namespace MoneyManager_DesktopApp.Views;

public partial class CategoryTab : UserControl
{
    public CategoryTab()
    {
        InitializeComponent();
        DataContext = new CategoryTabViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
}