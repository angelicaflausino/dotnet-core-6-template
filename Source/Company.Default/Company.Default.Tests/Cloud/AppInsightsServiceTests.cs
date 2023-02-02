using Company.Default.Cloud.Insights;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NPOI.OpenXml4Net.OPC;

namespace Company.Default.Tests.Cloud
{
    public class AppInsightsServiceTests
    {
        private readonly AppInsightsService _appInsightsService;
        private const string message = "Test track event";
        private readonly IConfiguration configuration;

        public AppInsightsServiceTests()
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json")
                .Build();
            _appInsightsService = GetAppInsightsService(configuration);
        }

        [Fact]
        public void LogInsights_LogLevel_Critical()
        {
            _appInsightsService.LogInformation(message);
        }

        [Fact]
        public void LogInsights_LogLevel_TrackEvent()
        {
            _appInsightsService.TrackEvent(message);
        }


        public static AppInsightsService GetAppInsightsService(IConfiguration configuration)
        {
            var instrumentationKey = configuration.GetValue<string>("ApplicationInsights:ConnectionString");

            var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
            telemetryConfiguration.ConnectionString = instrumentationKey;
            var telemetryClient = new TelemetryClient(telemetryConfiguration);
            telemetryClient.TrackTrace(message);

            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<AppInsightsServiceTests>();
            return new AppInsightsService(telemetryClient, logger);
        }
    }
}
