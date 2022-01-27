using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HappyTravel.SupplierOptionsProvider.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HappyTravel.SupplierOptionsProvider
{
    internal class SupplierOptionsUpdater : BackgroundService
    {
        public SupplierOptionsUpdater(IHttpClientFactory httpClientFactory, IServiceScopeFactory scopeFactory, 
            IOptions<Configuration> configuration, ISupplierOptionsStorage storage)
        {
            _httpClientFactory = httpClientFactory;
            _scopeFactory = scopeFactory;
            _configuration = configuration.Value;
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
                    await UpdateStorage(logger, stoppingToken);
                }
                catch (Exception)
                {
                    logger.LogSupplierStorageUpdateFailed();
                }

                await Task.Delay(_configuration.UpdaterInterval, stoppingToken);
            }
        }


        private async Task UpdateStorage(ILogger logger, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient(_configuration.HttpClientName);
            using var response = await client.GetAsync(_configuration.Endpoint, cancellationToken);
            
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var suppliers = await JsonSerializer.DeserializeAsync<List<Supplier>>(stream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }, cancellationToken) ?? new List<Supplier>();
            
            _storage.Set(suppliers);
            logger.LogSuppliersStorageRefreshed(suppliers.Count);
        }

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ISupplierOptionsStorage _storage;
        private readonly Configuration _configuration;
    }
}