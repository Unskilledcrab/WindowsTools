using WT.FileScanner.UI.Shared;
using WT.FileScanner.UI.Shared.Services.Interfaces;
using WT.FileScanner.WPF.Services;

namespace WT.FileScanner.WPF
{
    public static class Configure
    {
        public static void ConfigureServices()
        {
            AddServices();
            AddViews();
        }

        private static void AddViews()
        {
            IoC.AddService<MainWindow>(LifetimeType.Singleton);
            IoC.AddService<UserControls.FileScanner>(LifetimeType.Singleton);
        }

        private static void AddServices()
        {
            IoC.AddService<IUIManager, UIManager>(LifetimeType.Scoped);
        }
    }
}