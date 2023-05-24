using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MoneyManager_DesktopApp.ViewModels;
using MoneyManager_DesktopApp.Views;

namespace MoneyManager_DesktopApp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var bootstrapper = new AppBootstrapper();
            //bootstrapper.Initialize();
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                WindowState = WindowState.Maximized
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}