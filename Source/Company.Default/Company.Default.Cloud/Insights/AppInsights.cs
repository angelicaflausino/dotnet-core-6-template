using Company.Default.Cloud.Interfaces;

namespace Company.Default.Cloud.Insights
{
    public class AppInsights 
    {
        private readonly IAppInsightsService _appInsightsService;

        public AppInsights(IAppInsightsService appInsights)
        {
            _appInsightsService = appInsights;
        }
    }
}