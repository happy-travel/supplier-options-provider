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
            if (Uri.IsWellFormedUriString(supplierOptionsConfig.Endpoint, UriKind.Absolute))
                throw new ArgumentException($"Invalid supplier client endpoint {supplierOptionsConfig.Endpoint}");
            
            if (string.IsNullOrWhiteSpace(supplierOptionsConfig.IdentityClientName))
                throw new ArgumentException($"Identity client name cannot be null");

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