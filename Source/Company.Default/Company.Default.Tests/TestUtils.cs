using Microsoft.Extensions.Configuration;

namespace Company.Default.Tests
{
    public static class TestUtils
    {
        public static IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json")
                .Build();

            return config;
        }
    }
}
