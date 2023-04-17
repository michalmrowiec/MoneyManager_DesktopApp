using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MoneyManager_DesktopApp.Views;

public partial class StartTab : UserControl
{
    public StartTab()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}