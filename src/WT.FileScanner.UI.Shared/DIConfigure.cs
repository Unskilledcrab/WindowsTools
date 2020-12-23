using Microsoft.Extensions.DependencyInjection;

namespace WT.FileScanner.UI.Shared
{
    public static class DIConfigure
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection services)
        {
            services.AddViewModels();
            return services;
        }

        /// <summary>
        /// Configures all of the view models for dependency injection in this list
        /// </summary>
        /// <param name="services"></param>
        private static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            return services;
        }
    }
}