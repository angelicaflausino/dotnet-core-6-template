using Company.Default.Cloud.Interfaces;
using Company.Default.Cloud.KeyVault;
using Company.Default.Cloud.Storage;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CloudConfigurationExtensions
    {
        private static IConfiguration _configuration;

        public static void AddCloud(this IServiceCollection services, IConfiguration configuration)
        {
            _configuration = configuration;

            string storageConnectionString = GetStorageConnectionStrings();
            Uri keyVaultUri = new Uri(GetKeyVaultUri());

            services.AddAzureClients(builder =>
            {
                //KeyVault
                builder.AddSecretClient(keyVaultUri);

                //Storage Account
                //Blob
                builder.AddBlobServiceClient(storageConnectionString);
                //Queue
                builder.AddQueueServiceClient(storageConnectionString);
                //Table
                builder.AddTableServiceClient(storageConnectionString);
                
            });

            services.AddScoped<IBlobStorageService, BlobStorageService>();
            services.AddScoped<IKeyVaultService, KeyVaultService>();
            services.AddScoped<IQueueStorageService, QueueStorageService>();
            services.AddScoped<ITableStorageService, TableStorageService>();
        }

        private static string GetStorageConnectionStrings() => _configuration.GetSection("Storage").Value ?? string.Empty;

        private static string GetKeyVaultUri() => _configuration.GetSection("KeyVault:VaultUri").Value ?? string.Empty;
            
    }
}
