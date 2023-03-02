using $safeprojectname$.Interfaces;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;

namespace $safeprojectname$.Insights
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

        public void LogCritical(string? message, params object?[] args)
        {
            _logger.LogCritical(message, args);
        }

        public void LogError(string message, params object?[] args)
        {
            _logger.LogError(message, args);
        }

        public void LogError(Exception exception, EventId eventId, string message, params object?[] args)
        {
            _logger.LogError(eventId, exception, message, args);
        }

        public void LogException(Exception exception, params object?[] args)
        {
            _logger.LogError(exception, exception.Message, args);
        }

        public void LogInformation(string message, params object?[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogTrace(string? message, params object?[] args)
        {
            _logger.LogTrace(message, args);
        }

        public void LogTrace(EventId eventId, Exception? exception, string? message, params object?[] args)
        {
            _logger.LogTrace(eventId, exception, message, args);
        }

        public void LogWarning(string? message, params object?[] args)
        {
            _logger.LogWarning(message, args);
        }

        public void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            _telemetryClient.TrackEvent(eventName, properties, metrics);
            Flush();
        }

        public void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            _telemetryClient.TrackException(exception, properties, metrics);
            Flush();
        }

        public void TrackRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success)
        {
            _telemetryClient.TrackRequest(name, startTime, duration, responseCode, success);
            Flush();
        }

        private void Flush() => _telemetryClient.Flush();
    }
}
