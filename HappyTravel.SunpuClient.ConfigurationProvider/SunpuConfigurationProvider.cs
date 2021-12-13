using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HappyTravel.SunpuClient.ConfigurationProvider
{
    public class SunpuConfigurationProvider: Microsoft.Extensions.Configuration.ConfigurationProvider
    {
        public SunpuConfigurationProvider(string endpoint)
        {
            _endpoint = endpoint;
            _client = new HttpClient();
        }
    
        public override void Load()
        {
            Data = Generate(Fetch().GetAwaiter().GetResult());
        }

        private async Task<List<Supplier>> Fetch()
        {
            using var response = await _client.GetAsync(_endpoint);
            response.EnsureSuccessStatusCode();

            var stream =  await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<List<Supplier>>(stream, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                })
                   ?? new List<Supplier>();
        }


        private static Dictionary<string, string> Generate(List<Supplier> suppliers)
        {
            var dictionary = new Dictionary<string, string>();

            for (var i = 0; i < suppliers.Count; i++)
            {
                dictionary.Add($"{SectionName}:{ListName}:{i}:{nameof(Supplier.Id)}", suppliers[i].Id.ToString());
                dictionary.Add($"{SectionName}:{ListName}:{i}:{nameof(Supplier.Name)}", suppliers[i].Name);
                dictionary.Add($"{SectionName}:{ListName}:{i}:{nameof(Supplier.IsEnabled)}", suppliers[i].IsEnabled.ToString());
                dictionary.Add($"{SectionName}:{ListName}:{i}:{nameof(Supplier.ConnectorUrl)}", suppliers[i].ConnectorUrl);
            }

            return dictionary;
        }


        public const string SectionName = "SuppliersOptions";
        private const string ListName = "Suppliers";


        private readonly string _endpoint;
        private readonly HttpClient _client;
    }
}