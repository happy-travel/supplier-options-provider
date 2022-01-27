using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HappyTravel.SunpuClient.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HappyTravel.SunpuClient
{
    public class Updater : BackgroundService
    {
        public Updater(IHttpClientFactory httpClientFactory, IServiceScopeFactory scopeFactory, IOptions<Configuration> configuration, IStorage storage, ILogger<Updater> logger)
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
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Updater>>();
                
                try
                {
                    await UpdateStorage(logger, stoppingToken);
                }
                catch (Exception)
                {
                    logger.LogSupplierStorageUpdateFailed();
                }

                await Task.Delay(_configuration.UpdaterIntervalInSeconds, stoppingToken);
            }
        }


        private async Task UpdateStorage(ILogger logger, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient(_configuration.HttpClientName);
            using var response = await client.GetAsync(_configuration.EndPoint, cancellationToken);
            
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
        private readonly Configuration _configuration;
        private readonly IStorage _storage;
    }
}