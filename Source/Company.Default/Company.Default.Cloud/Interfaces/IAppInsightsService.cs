namespace Company.Default.Cloud.Interfaces
{
    public interface IAppInsightsService
    {
        void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);
        void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);
    }
}
