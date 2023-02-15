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

        public static IMapper GetAutoMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(Assembly.GetAssembly(typeof(EntityToDtoProfile)));
            });
            configuration.AssertConfigurationIsValid();

            return configuration.CreateMapper();
        }
    }
}
