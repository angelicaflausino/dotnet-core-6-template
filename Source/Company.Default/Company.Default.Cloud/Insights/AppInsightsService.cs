using Company.Default.Cloud.Interfaces;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;

namespace Company.Default.Cloud.Insights
{
    public class AppInsightsService : IAppInsightsService
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly ILogger<AppInsightsService> _logger;
        public AppInsightsService(TelemetryClient telemetryClient, ILoggerFactory loggerFactory)
        {
            _telemetryClient = telemetryClient;
            _logger = loggerFactory.CreateLogger<AppInsightsService>();
        }

        public void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            _telemetryClient.TrackEvent(eventName, properties, metrics);
        }
        public void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            _telemetryClient.TrackException(exception, properties, metrics);
        }
    }
}
