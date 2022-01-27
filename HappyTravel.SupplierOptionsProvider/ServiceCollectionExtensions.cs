using System;
using Microsoft.Extensions.DependencyInjection;

namespace HappyTravel.SupplierOptionsProvider
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSupplierOptionsProvider(this IServiceCollection services, Action<Configuration> configuration)
        {
            services.Configure(configuration);
            services.AddHostedService<SupplierOptionsUpdater>();
            services.AddSingleton<ISupplierOptionsStorage, SupplierOptionsStorage>();
            return services;
        }
    }
}