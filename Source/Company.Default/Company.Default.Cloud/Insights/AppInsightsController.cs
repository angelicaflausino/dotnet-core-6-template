using Company.Default.Cloud.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Company.Default.Cloud.Insights
{
    [ApiController]
    public class AppInsightsController : ControllerBase
    {
        private readonly IAppInsightsService _appInsights;

        public AppInsightsController(IAppInsightsService appInsights)
        {
            _appInsights = appInsights;
        }

        [HttpGet]
        public ActionResult<string> LogInsight()
        {
            try
            {
                _appInsights.TrackEvent("That's work!");
                return Ok("Logged on insights");
            }
            catch (Exception exception)
            {
                _appInsights.TrackException(exception);
                return StatusCode(500, "An error ocurred");
            }
        }
    }
}
