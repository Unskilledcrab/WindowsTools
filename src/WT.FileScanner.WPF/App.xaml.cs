using System.Windows;
using WT.FileScanner.UI.Shared;

namespace WT.FileScanner.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IoC.Create();
            Configure.ConfigureServices();
            IoC.Build();
            var mainWindow = IoC.Get<MainWindow>();
            mainWindow.Show();
        }
    }
}