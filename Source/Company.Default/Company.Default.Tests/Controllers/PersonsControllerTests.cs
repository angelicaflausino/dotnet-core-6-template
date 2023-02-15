using Company.Default.Api.Controllers;
using Company.Default.Domain.Contracts.Base;
using Company.Default.Domain.Contracts.Services;
using Company.Default.Domain.Dtos;
using Company.Default.Domain.Entities;
using Company.Default.Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Company.Default.Tests.Controllers
{
    public class PersonsControllerTests
    {
        private readonly int _success = 200;
        private readonly int _noContent = 204;
        private readonly int _badRequest = 400;

        [Fact]
        public void Get_Success()
        {
            long id = 1;
            var mockPersonService = GetMockPersonService();
            mockPersonService.Setup(x => x.GetPerson(It.IsAny<long>())).Returns(new PersonDto { Id = id });
            var mockCrudService = GetMockCrudService();
            var controller = new PersonsController(mockPersonService.Object, mockCrudService.Object);

            var actionResult = controller.Get(id);
            var result = actionResult as OkObjectResult;
                      
            Assert.NotNull(result);
            Assert.Equal(_success, result?.StatusCode);
        }

        [Fact]
        public void GetSearch_Success()
        {
            var filter = new PersonFilterParameter { Page = 1, Size = 20 };
            var paged = new PagedResultDto<PersonDto> { Result = new List<PersonDto>(), Total = 0 };
            var mockPersonService = GetMockPersonService();
            mockPersonService.Setup(x => x.GetPagedSearch(filter)).Returns(paged);
            var mockCrudService = GetMockCrudService();
            var controller = new PersonsController(mockPersonService.Object, mockCrudService.Object);

            var actionResult = controller.GetSearch(filter);
            var result = actionResult as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(_success, result?.StatusCode);
        }

        [Fact]
        public void GetAll_Success()
        {
            var mockPersonService = GetMockPersonService();
            mockPersonService.Setup(x => x.GetAll()).Returns(new List<PersonDto>());
            var mockCrudService = GetMockCrudService();
            var controller = new PersonsController(mockPersonService.Object, mockCrudService.Object);

            var actionResult = controller.GetAll();
            var result = actionResult as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(_success, result?.StatusCode);
        }

        [Fact]
        public void Post_Success()
        {
            var mockPersonService = GetMockPersonService();
            mockPersonService.Setup(x => x.MapFromDto(It.IsAny<PersonDto>())).Returns(new Person());
            mockPersonService.Setup(x => x.Validate(It.IsAny<Person>())).Returns(new ValidatorResult(true, new List<ValidatorError>()));
            var mockCrudService = GetMockCrudService();
            mockCrudService.Setup(x => x.Create(It.IsAny<Person>())).Returns(new Person());
            var controller = new PersonsController(mockPersonService.Object, mockCrudService.Object);

            var actionResult = controller.Post(new PersonDto { Id = 1 });
            var result = actionResult as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(_success, result?.StatusCode);
        }

        [Fact]
        public void Post_BadRequest()
        {
            var mockPersonService = GetMockPersonService();
            mockPersonService.Setup(x => x.MapFromDto(It.IsAny<PersonDto>())).Returns(new Person());
            mockPersonService.Setup(x => x.Validate(It.IsAny<Person>())).Returns(new ValidatorResult(false, new List<ValidatorError>()));
            var mockCrudService = GetMockCrudService();
            var controller = new PersonsController(mockPersonService.Object, mockCrudService.Object);

            var actionResult = controller.Post(new PersonDto { Id = 1 });
            var result = actionResult as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(_badRequest, result?.StatusCode);
        }

        [Fact]
        public void Put_Success()
        {
            var mockPersonService = GetMockPersonService();
            mockPersonService.Setup(x => x.MapFromDto(It.IsAny<PersonDto>())).Returns(new Person());
            mockPersonService.Setup(x => x.Validate(It.IsAny<Person>())).Returns(new ValidatorResult(true, new List<ValidatorError>()));
            var mockCrudService = GetMockCrudService();
            mockCrudService.Setup(x => x.Update(It.IsAny<Person>())).Verifiable();
            var controller = new PersonsController(mockPersonService.Object, mockCrudService.Object);

            var actionResult = controller.Put(new PersonDto { Id = 1 });
            var result = actionResult as NoContentResult;

            Assert.NotNull(result);
            Assert.Equal(_noContent, result?.StatusCode);
        }

        [Fact]
        public void Put_BadRequest()
        {
            var mockPersonService = GetMockPersonService();
            mockPersonService.Setup(x => x.MapFromDto(It.IsAny<PersonDto>())).Returns(new Person());
            mockPersonService.Setup(x => x.Validate(It.IsAny<Person>())).Returns(new ValidatorResult(false, new List<ValidatorError>()));
            var mockCrudService = GetMockCrudService();
            var controller = new PersonsController(mockPersonService.Object, mockCrudService.Object);

            var actionResult = controller.Put(new PersonDto { Id = 1 });
            var result = actionResult as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(_badRequest, result?.StatusCode);
        }

        [Fact]
        public void Delete_Success() 
        {
            var id = 1;
            var mockPersonService = GetMockPersonService();
            var mockCrudService = GetMockCrudService();
            mockCrudService.Setup(x => x.Delete(It.IsAny<long>())).Returns(true);
            var controller = new PersonsController(mockPersonService.Object, mockCrudService.Object);

            var actionResult = controller.Delete(id);
            var result = actionResult as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(_success, result?.StatusCode);
        }

        #region Mocking
        private Mock<IPersonService> GetMockPersonService() =>
            new Mock<IPersonService>();

        private Mock<ICrudService<Person, long>> GetMockCrudService() =>
            new Mock<ICrudService<Person, long>>();
        
        #endregion
    }
}
