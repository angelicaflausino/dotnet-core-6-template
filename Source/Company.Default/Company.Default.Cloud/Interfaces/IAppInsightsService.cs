using Microsoft.Extensions.Logging;

namespace Company.Default.Cloud.Interfaces
{
    public interface IAppInsightsService
    {
        /// <summary>
        /// Use TelemetryClient to track custom event.
        /// <para>
        /// <see href="https://learn.microsoft.com/en-us/azure/azure-monitor/app/api-custom-events-metrics#trackevent">Go to Microsoft Learn</see>
        /// </para>
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="properties"></param>
        /// <param name="metrics"></param>

        void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

        /// <summary>
        /// Use TelemetryClient to track request.
        /// <para>
        /// <see href="https://learn.microsoft.com/en-us/azure/azure-monitor/profiler/profiler-trackrequests">Go to Microsoft Learn</see>
        /// </para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="startTime"></param>
        /// <param name="duration"></param>
        /// <param name="responseCode"></param>
        /// <param name="success"></param>
        void TrackRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success);

        /// <summary>
        /// Use TelemetryClient to track exception.
        /// <para>
        /// <see href="https://learn.microsoft.com/en-us/azure/azure-monitor/app/asp-net-exceptions#report-exceptions-explicitly">Go to Microsoft Learn</see>
        /// </para>
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="properties"></param>
        /// <param name="metrics"></param>
        void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

        /// <summary>
        /// Use ILogger.LogError() to log Exception
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="args"></param>
        void LogException(Exception exception, params object?[] args);

        /// <summary>
        /// Use ILogger.LogTrace() to log Trace
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void LogTrace(string? message, params object?[] args);

        /// <summary>
        /// Use ILogger.LogTrace() to log Trace 
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <param name="arg"></param>
        void LogTrace(EventId eventId, Exception? exception, string? message, params object?[] arg);

        /// <summary>
        /// Use ILogger.LogError() to log Error
        /// </summary>
        /// <param name="error"></param>
        /// <param name="args"></param>
        void LogError(string error, params object?[] args);

        /// <summary>
        /// Use ILogger.LogError()
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="eventId"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void LogError(Exception exception, EventId eventId, string message, params object?[] args);

        /// <summary>
        /// Use ILogger.LogInformation() to log Information
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void LogInformation(string message, params object?[] args);

        /// <summary>
        /// Use ILogger.LogWarning() to log Warning
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void LogWarning(string? message, params object?[] args);

        /// <summary>
        /// Use ILogger.LogCritical() to log Critical
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void LogCritical(string? message, params object?[] args);
    }
}
