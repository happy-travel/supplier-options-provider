﻿using System;
using Microsoft.Extensions.Configuration;

namespace HappyTravel.SunpuClient.ConfigurationProvider
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddSuppliersConfiguration(this IConfigurationBuilder configuration, 
            string endpoint, string identityUrl, string clientId, string clientSecret, string clientScope)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ArgumentException("Endpoint is required", nameof(endpoint));
            
            return configuration.Add(new SunpuConfigurationSource(endpoint, identityUrl, clientId, clientSecret, clientScope));
        }
    }
}