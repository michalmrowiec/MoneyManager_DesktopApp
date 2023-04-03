using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MoneyManager_DesktopApp.ViewModels;

namespace MoneyManager_DesktopApp.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
// #if DEBUG
//         this.AttachDevTools();
// #endif
    }

    // private void InitializeComponent()
    // {
    //     AvaloniaXamlLoader.Load(this);
    // }
}