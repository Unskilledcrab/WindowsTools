using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace WT.FileScanner.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Container.Create();
            var mainWindow = Container.ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}