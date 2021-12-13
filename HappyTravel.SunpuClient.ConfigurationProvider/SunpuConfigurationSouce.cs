using Microsoft.Extensions.Configuration;

namespace HappyTravel.SunpuClient.ConfigurationProvider
{
    public class SunpuConfigurationSource : IConfigurationSource
    {
        public SunpuConfigurationSource(string endpoint)
        {
            _endpoint = endpoint;
        }
    
    
        public IConfigurationProvider Build(IConfigurationBuilder builder) 
            => new SunpuConfigurationProvider(_endpoint);


        private readonly string _endpoint;
    }
}