using Microsoft.Extensions.DependencyInjection;
using System;
using WT.FileScanner.UI.Shared.Services.Interfaces;

namespace WT.FileScanner.UI.Shared
{
    public enum LifetimeType { Singleton, Scoped, Transient }

    public static class IoC
    {
        private static ServiceCollection serviceCollection;
        public static IServiceProvider ServiceProvider { get; private set; }
        public static IUIManager UI => Get<IUIManager>();

        public static T Get<T>() => ServiceProvider.GetRequiredService<T>();

        public static void Create()
        {
            if (serviceCollection == null) serviceCollection = new ServiceCollection();
            AddViewModels(serviceCollection);
        }

        /// <summary>
        /// Used to add specific UI framework implementations before <see cref="Build"/> to add
        /// services for the shared interfaces.
        /// </summary>
        /// <remarks>Must use before <see cref="Build"/> or will not be included in IoC Container</remarks>
        /// <typeparam name="TService">shared interface of the service</typeparam>
        /// <typeparam name="TImplementation">implementation of the service</typeparam>
        /// <param name="lifetime">The lifetime of the service</param>
        public static void AddService<TService, TImplementation>(LifetimeType lifetime)
            where TService : class
            where TImplementation : class, TService
        {
            if (serviceCollection == null) Create();
            switch (lifetime)
            {
                case LifetimeType.Singleton:
                    serviceCollection.AddSingleton<TService, TImplementation>();
                    break;

                case LifetimeType.Scoped:
                    serviceCollection.AddScoped<TService, TImplementation>();
                    break;

                case LifetimeType.Transient:
                    serviceCollection.AddTransient<TService, TImplementation>();
                    break;

                default:
                    break;
            }
        }

        public static void AddService<TService>(LifetimeType lifetime)
            where TService : class
        {
            if (serviceCollection == null) Create();
            switch (lifetime)
            {
                case LifetimeType.Singleton:
                    serviceCollection.AddSingleton<TService>();
                    break;

                case LifetimeType.Scoped:
                    serviceCollection.AddScoped<TService>();
                    break;

                case LifetimeType.Transient:
                    serviceCollection.AddTransient<TService>();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Build the IoC container at application startup
        /// </summary>
        /// <remarks>
        /// add any framework specific services using <see cref="AddService{TService,
        /// TImplementation}(LifetimeType)"/> before building
        /// </remarks>
        public static void Build()
        {
            if (serviceCollection == null) Create();
            ServiceProvider = serviceCollection.BuildServiceProvider();
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