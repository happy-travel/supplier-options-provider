using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace HappyTravel.SunpuClient
{
    public class SunpuService
    {
        public SunpuService(IHttpClientFactory clientFactory, IOptions<SunpuClientOptions> options)
        {
            _clientFactory = clientFactory;
            _options = options.Value;
        }

        
        public async Task<List<Supplier>> GetSuppliers()
        {
            var client = _clientFactory.CreateClient(_options.HttpClientName);
            using var response = await client.GetAsync(_options.EndPoint);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<List<Supplier>>(stream, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                })
                   ?? new List<Supplier>();
        }
        
        
        private readonly IHttpClientFactory _clientFactory;
        private readonly SunpuClientOptions _options;
    }
}