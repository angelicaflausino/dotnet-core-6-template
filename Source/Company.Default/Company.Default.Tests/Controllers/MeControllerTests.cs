using Company.Default.Api.Controllers;
using Company.Default.Cloud.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Company.Default.Tests.Controllers
{
    public class MeControllerTests
    {
        private readonly int _success = 200;

        [Fact]
        public async Task Get_Success()
        {
            var mockGraphService = GetMockGraphService();
            var controller = new MeController(mockGraphService.Object);

            var actionResult = await controller.Get();
            var result = actionResult as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(_success, result?.StatusCode);
        }

        [Fact]
        public async Task GetProperties_Success()
        {
            var mockGraphService = GetMockGraphService();
            var controller = new MeController(mockGraphService.Object);

            var actionResult = await controller.GetProperties(new string[] {"Name", "Surname"});
            var result = actionResult as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(_success, result?.StatusCode);
        }

        [Fact]
        public async Task GetPhoto_Success()
        {
            var mockGraphService = GetMockGraphService();
            var controller = new MeController(mockGraphService.Object);

            var actionResult = await controller.GetPhoto();
            var result = actionResult as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(_success, result?.StatusCode);
        }

        #region Mocking
        private Mock<IGraphMeService> GetMockGraphService()
        {
            var mock = new Mock<IGraphMeService>();

            mock.Setup(x => x.GetAsync(It.IsAny<string[]>())).ReturnsAsync(new { Id = 1, Name = "Foo", Surname = "Bar" });
            mock.Setup(x => x.GetProfilePhotoAsBase64()).ReturnsAsync("data:image/jpeg, base64, /9864dsd");

            return mock;
        }
        #endregion
    }
}
