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
        public SupplierOptionsUpdater(ISupplierOptionsClient supplierOptionsClient, IServiceScopeFactory scopeFactory, 
            IOptions<SupplierOptionsProviderConfiguration> configuration, ISupplierOptionsStorage storage)
        {
            _supplierOptionsClient = supplierOptionsClient;
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
                
                try
                {
                    await UpdateStorage(logger);
                }
                catch (Exception ex)
                {
                    logger.LogSupplierStorageUpdateFailed(ex, ex.Message);
                }

                await Task.Delay(_supplierOptionsProviderConfiguration.UpdaterInterval, stoppingToken);
            }
        }


        private async Task UpdateStorage(ILogger logger)
        {
            var (_, isFailure, suppliers, error) = await _supplierOptionsClient.GetAll();
            if (isFailure)
                throw new Exception($"Supplier options storage update failed: {error}");
            
            _storage.Set(suppliers);
            logger.LogSuppliersStorageRefreshed(suppliers.Count);
        }

        private readonly ISupplierOptionsClient _supplierOptionsClient;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ISupplierOptionsStorage _storage;
        private readonly SupplierOptionsProviderConfiguration _supplierOptionsProviderConfiguration;
    }
}