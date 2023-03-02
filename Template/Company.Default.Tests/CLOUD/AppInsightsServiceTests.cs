using $ext_safeprojectname$.Cloud.Insights;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace $safeprojectname$.Cloud
{
    public class AppInsightsServiceTests
    {
        private readonly AppInsightsService _appInsightsService;
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
            var message = "Unit Test LogCritical";
            _appInsightsService.LogCritical(message);
        }

        [Fact]
        public void LogInsights_LogLevel_Error()
        {
            var message = "Unit Test LogError";
            _appInsightsService.LogError(message);
        }

        [Fact]
        public void LogInsights_LogLevel_Exception()
        {
            var exception = new Exception("Unit Test LogException");
            _appInsightsService.LogException(exception);
        }

        [Fact]
        public void LogInsights_LogLevel_Information()
        {
            var message = "Unit Test LogInformation";
            _appInsightsService.LogInformation(message);
        }

        [Fact]
        public void LogInsights_LogLevel_Trace()
        {
            var message = "Unit Test LogTrace";
            _appInsightsService.LogTrace(message);
        }

        [Fact]
        public void LogInsights_LogLevel_Warning()
        {
            var message = "Unit Test LogWarning";
            _appInsightsService.LogWarning(message);
        }

        [Fact]
        public void LogInsights_LogLevel_TrackEvent()
        {
            var message = "Unit Test TrackEvent";
            _appInsightsService.TrackEvent(message);
        }

        [Fact]
        public void LogInsights_LogLevel_TrackException()
        {
            var exception = new Exception("Unit Test TrackException");
            _appInsightsService.TrackException(exception);
        }

        [Fact]
        public void LogInsights_LogLevel_TrackRequest()
        {
            var message = "Unit Test TrackRequest";
            var responseCode = "200";
            _appInsightsService.TrackRequest(message, DateTime.Now, TimeSpan.Zero, responseCode, true);
        }

        public static AppInsightsService GetAppInsightsService(IConfiguration configuration)
        {
            var instrumentationKey = configuration.GetValue<string>("ApplicationInsights:ConnectionString");

            var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
            telemetryConfiguration.ConnectionString = instrumentationKey;
            var telemetryClient = new TelemetryClient(telemetryConfiguration);

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            return new AppInsightsService(telemetryClient, loggerFactory);
        }
    }
}
