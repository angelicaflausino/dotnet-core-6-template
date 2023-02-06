using Microsoft.Extensions.Logging;

namespace Company.Default.Cloud.Interfaces
{
    public interface IAppInsightsService
    {
        void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);
        void TrackRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success);
        void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);
        void LogException(Exception exception, params object?[] args);
        void LogTrace(string? message, params object?[] args);
        void LogTrace(EventId eventId, Exception? exception, string? message, params object?[] arg);
        void LogError(string error, params object?[] args);
        void LogError(Exception exception, EventId eventId, string message, params object?[] args);
        void LogInformation(string message, params object?[] args);
        void LogWarning(string? message, params object?[] args);
        void LogCritical(string? message, params object?[] args);
    }
}
