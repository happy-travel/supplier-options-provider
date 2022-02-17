using System;
using HappyTravel.SupplierOptionsClient;
using Microsoft.Extensions.DependencyInjection;

namespace HappyTravel.SupplierOptionsProvider
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSupplierOptionsProvider(this IServiceCollection services, 
            Action<SupplierOptionsProviderConfiguration> configurationAction)
        {
            services.Configure(configurationAction);
            services.AddHostedService<SupplierOptionsUpdater>();

            var supplierOptionsConfig = new SupplierOptionsProviderConfiguration();
            configurationAction(supplierOptionsConfig);
            if (string.IsNullOrWhiteSpace(supplierOptionsConfig.Endpoint))
                throw new ArgumentException($"Supplier client endpoint cannot be empty");
            
            if (string.IsNullOrWhiteSpace(supplierOptionsConfig.IdentityClientName))
                throw new ArgumentException($"Identity client name cannot be empty");

            if (supplierOptionsConfig.StorageTimeout == default)
                throw new ArgumentException("Storage timeout is not set");
            
            if (supplierOptionsConfig.UpdaterInterval == default)
                throw new ArgumentException("Updater interval is not set");
            
            services.AddSingleton<ISupplierOptionsStorage, SupplierOptionsStorage>();
            services.AddSupplierOptionsClient(settings =>
            {
                settings.Endpoint = supplierOptionsConfig.Endpoint;
            }, supplierOptionsConfig.IdentityClientName);
            
            return services;
        }
    }
}