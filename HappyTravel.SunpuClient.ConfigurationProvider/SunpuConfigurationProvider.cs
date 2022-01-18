using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace HappyTravel.SunpuClient.ConfigurationProvider
{
    public class SunpuConfigurationProvider : Microsoft.Extensions.Configuration.ConfigurationProvider
    {
        public SunpuConfigurationProvider(string endpoint, 
            string identityUrl, 
            string clientId, 
            string clientSecret,
            string clientScope
            )
        {
            _endpoint = endpoint;
            _client = new HttpClient();
            _recurrentLoadTask = new Task(LoadSuppliers);
            _identityUrl = identityUrl;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _clientScope = clientScope;
        }
    
        public override void Load()
        {
            _recurrentLoadTask.Start();
        }

        
        private async void LoadSuppliers()
        {
            await LoadIdentityToken();
            
            while (true)
            {
                try
                {
                    Data = Generate(await Fetch());
                    OnReload();
                    await Task.Delay(RecurrentLoadDelayInMilliseconds);
                }
                catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await LoadIdentityToken();
                }
                catch { }
            }
        }


        private async Task LoadIdentityToken()
        {
            var data = new FormUrlEncodedContent(new List<KeyValuePair<string?, string?>>
            {
                new("client_id", _clientId),
                new("client_secret", _clientSecret),
                new("client_scope", _clientScope),
                new("grant_type", "client_credentials")
            });
                
            using var response = await _client.PostAsync(_identityUrl, data);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();

            _identityToken = JsonDocument.Parse(content)
                .RootElement
                .GetProperty("access_token")
                .GetString();
        }
        

        private async Task<List<Supplier>> Fetch()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _identityToken);
            using var response = await _client.GetAsync(_endpoint);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
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
                dictionary.Add($"{SectionName}:{ListName}:{i}:{nameof(Supplier.IsMultiRoomFlowSupported)}", suppliers[i].IsMultiRoomFlowSupported.ToString());
            }

            return dictionary;
        }


        public const string SectionName = "SuppliersOptions";
        private const string ListName = "Suppliers";
        private const int RecurrentLoadDelayInMilliseconds = 60000;

        private readonly string _endpoint;
        private readonly HttpClient _client;
        private readonly Task _recurrentLoadTask;
        private readonly string _identityUrl;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _clientScope;
        private string _identityToken;
    }
}