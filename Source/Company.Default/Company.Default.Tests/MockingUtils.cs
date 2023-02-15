using Company.Default.Cloud.Interfaces;

namespace Company.Default.Tests
{
    public static class MockingUtils
    {
        public static Mock<IAppInsightsService> GetMockAppInsightsService()
        {
            var mock = new Mock<IAppInsightsService>();

            mock.Setup(x => x.TrackEvent(It.IsAny<string>(),
                It.IsAny<IDictionary<string, string>>(),
                It.IsAny<IDictionary<string, double>>())).Verifiable();

            mock.Setup(x => x.TrackRequest(It.IsAny<string>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<string>(),
                It.IsAny<bool>())).Verifiable();

            mock.Setup(x => x.TrackException(It.IsAny<Exception>(),
                It.IsAny<IDictionary<string, string>>(),
                It.IsAny<IDictionary<string, double>>())).Verifiable();

            mock.Setup(x => x.LogException(It.IsAny<Exception>())).Verifiable();

            mock.Setup(x => x.LogTrace(It.IsAny<string>())).Verifiable();

            mock.Setup(x => x.LogError(It.IsAny<string>())).Verifiable();

            mock.Setup(x => x.LogError(It.IsAny<Exception>(),
                It.IsAny<Microsoft.Extensions.Logging.EventId>(),
                It.IsAny<string>())).Verifiable();

            mock.Setup(x => x.LogInformation(It.IsAny<string>())).Verifiable();

            mock.Setup(x => x.LogWarning(It.IsAny<string>())).Verifiable();

            mock.Setup(x => x.LogCritical(It.IsAny<string>())).Verifiable();

            return mock;
        }
    }
}
