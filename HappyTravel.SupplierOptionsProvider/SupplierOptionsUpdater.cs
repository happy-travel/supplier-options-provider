using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.SupplierOptionsClient;
using HappyTravel.SupplierOptionsProvider.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HappyTravel.SupplierOptionsProvider
{
    internal class SupplierOptionsUpdater : BackgroundService
    {
        public SupplierOptionsUpdater(IServiceScopeFactory scopeFactory, ISupplierOptionsStorage storage,
            IOptions<SupplierOptionsProviderConfiguration> configuration)
        {
            _scopeFactory = scopeFactory;
            _supplierOptionsProviderConfiguration = configuration.Value;
            _storage = storage;
        }
        
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<SupplierOptionsUpdater>>();
                var client = scope.ServiceProvider.GetRequiredService<ISupplierOptionsClient>();

                try
                {
                    await UpdateStorage(client, logger);
                }
                catch (Exception ex)
                {
                    logger.LogSupplierStorageUpdateFailed(ex, ex.Message);
                }

                await Task.Delay(_supplierOptionsProviderConfiguration.UpdaterInterval, stoppingToken);
            }
        }


        private async Task UpdateStorage(ISupplierOptionsClient client, ILogger logger)
        {
            var (_, isFailure, suppliers, error) = await client.GetAll();
            if (isFailure)
                throw new Exception($"Supplier options storage update failed: {error}");
            
            _storage.Set(suppliers);
            logger.LogSuppliersStorageRefreshed(suppliers.Count);
        }

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ISupplierOptionsStorage _storage;
        private readonly SupplierOptionsProviderConfiguration _supplierOptionsProviderConfiguration;
    }
}