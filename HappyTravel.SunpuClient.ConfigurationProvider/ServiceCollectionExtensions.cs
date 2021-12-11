using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HappyTravel.SunpuClient.ConfigurationProvider
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureSuppliers(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<SupplierOptions>(configuration.GetSection(SunpuConfigurationProvider.SectionName));
        }
    }
}