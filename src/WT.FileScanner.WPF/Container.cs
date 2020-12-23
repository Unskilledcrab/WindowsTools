using Microsoft.Extensions.DependencyInjection;
using System;
using WT.FileScanner.UI.Shared;
using WT.FileScanner.WPF.ViewModels;

namespace WT.FileScanner.WPF
{
    public static class Container
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void Create()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private static IServiceCollection AddViews(this IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<UserControls.FileScanner>();
            return services;
        }

        private static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddScoped<FileScannerViewModel>();
            return services;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSharedServices();
            services.AddViewModels();
            services.AddViews();
        }
    }
}