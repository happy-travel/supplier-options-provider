using System;
using Microsoft.Extensions.DependencyInjection;

namespace HappyTravel.SunpuClient
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSupplierOptionsProvider(this IServiceCollection services, Action<Configuration> configuration)
        {
            services.Configure(configuration);
            services.AddHostedService<Updater>();
            services.AddSingleton<Storage>();
            return services;
        }
    }
}