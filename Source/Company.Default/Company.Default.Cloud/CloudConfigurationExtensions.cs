﻿using Company.Default.Cloud.Interfaces;
using Company.Default.Cloud.Storage;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CloudConfigurationExtensions
    {
        public static void AddCloud(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(configuration.GetSection("Storage"));
            });

            services.AddScoped<IBlobStorageService, BlobStorageService>();
        }
    }
}