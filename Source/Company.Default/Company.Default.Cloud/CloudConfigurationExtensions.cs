using Azure.Storage.Queues;
using Company.Default.Cloud.Graph;
using Company.Default.Cloud.Insights;
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

            services.AddAzureClients(builder =>
            {
                //KeyVault
                builder.AddSecretClient(new Uri(_keyVaultUri));

                //Storage Account
                //Blob
                builder.AddBlobServiceClient(_storageConnectionString);
                //Queue
                builder.AddQueueServiceClient(_storageConnectionString);
                //Table
                builder.AddTableServiceClient(_storageConnectionString); 
            });

            services.AddScoped<IBlobStorageService, BlobStorageService>();
            services.AddScoped<IKeyVaultService, KeyVaultService>();
            services.AddScoped<IQueueStorageService, QueueStorageService>(provider =>
            {
                var queueClient = new QueueClient(_storageConnectionString, _queueName);
                return new QueueStorageService(queueClient);
            });
            services.AddScoped<ITableStorageService, TableStorageService>();
            services.AddScoped<IAppInsightsService, AppInsightsService>();
            services.AddScoped<IGraphMeService, GraphMeService>();
        }

        private static string _storageConnectionString => _configuration.GetValue<string>("Storage:ConnectionString");

        private static string _keyVaultUri => _configuration.GetValue<string>("KeyVault:VaultUri");

        private static string _queueName => _configuration.GetValue<string>("Storage:QueueName");
            
    }
}
