using System;
using Microsoft.Extensions.DependencyInjection;

namespace HappyTravel.SunpuClient
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSunpuClient(this IServiceCollection services, Action<SunpuClientOptions> options)
        {
            services.Configure(options);
            services.AddSingleton<SunpuService>();
            return services;
        }
    }
}