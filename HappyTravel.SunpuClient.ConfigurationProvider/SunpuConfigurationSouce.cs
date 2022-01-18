using Microsoft.Extensions.Configuration;

namespace HappyTravel.SunpuClient.ConfigurationProvider
{
    public class SunpuConfigurationSource : IConfigurationSource
    {
        public SunpuConfigurationSource(string endpoint, 
            string identityUrl, 
            string clientId, 
            string clientSecret, 
            string clientScope)
        {
            _endpoint = endpoint;
            _identityUrl = identityUrl;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _clientScope = clientScope;
        }
    
    
        public IConfigurationProvider Build(IConfigurationBuilder builder) 
            => new SunpuConfigurationProvider(_endpoint, _identityUrl, _clientId, _clientSecret, _clientScope);


        private readonly string _endpoint;
        private readonly string _identityUrl;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _clientScope;
    }
}