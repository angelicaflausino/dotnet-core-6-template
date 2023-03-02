using $ext_safeprojectname$.Cloud.Interfaces;
using $ext_safeprojectname$.Core.Services;
using $ext_safeprojectname$.Core.Validations;
using $ext_safeprojectname$.Domain.Contracts.Repositories;
using $ext_safeprojectname$.Domain.Dtos;
using $ext_safeprojectname$.Domain.Entities;
using $ext_safeprojectname$.Domain.Filters;

namespace $safeprojectname$.Core
{
    public class PersonServiceTests
    {
        private static IMapper _mapper = TestUtils.GetAutoMapper();
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IAppInsightsService> _appInsightsService;
        private readonly IValidator<Person> _validator;
        private readonly PersonService _service;

        public PersonServiceTests()
        {
            _mockUow = GetMockUow();
            _validator = new PersonValidator();
            _appInsightsService = MockingUtils.GetMockAppInsightsService();
            _service = new PersonService(_mockUow.Object, _mapper, _validator, _appInsightsService.Object);
        }

        [Fact]
        public void GetAll_NotEmpty()
        {
            var persons = GetPersonsDto(10);
            _mockUow.Setup(x => x.Person.GetSelect(It.IsAny<Expression<Func<Person, bool>>>(),
                It.IsAny<Expression<Func<Person, PersonDto>>>(),
                It.IsAny<string[]>())).Returns(persons);
            
            var result = _service.GetAll();

            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetAll_ThrowsException()
        {
            _mockUow.Setup(x => x.Person.GetSelect(It.IsAny<Expression<Func<Person, bool>>>(),
                It.IsAny<Expression<Func<Person, PersonDto>>>(),
                It.IsAny<string[]>())).Throws(new Exception("Error"));

            var exception = Assert.Throws<Exception>(() => _service.GetAll());

            Assert.True(exception.Message == "Error");
            _appInsightsService.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        [Fact]
        public void GetPagedSearch_NotNull_NotEmpty()
        {
            var parameter = new PersonFilterParameter();
            var paged = GetPagedResult(10);

            _mockUow.Setup(x => x.Person.GetPaged(It.IsAny<Expression<Func<Person, bool>>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string[]>())).Returns(paged);

            var result = _service.GetPagedSearch(parameter);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Result);
        }

        [Fact]
        public void GetPagedSearch_ThrowsException()
        {
            var parameter = new PersonFilterParameter();
            _mockUow.Setup(x => x.Person.GetPaged(It.IsAny<Expression<Func<Person, bool>>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string[]>())).Throws(new Exception("Error"));

            var exception = Assert.Throws<Exception>(() => _service.GetPagedSearch(parameter));

            Assert.True(exception.Message == "Error");
            _appInsightsService.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        [Fact]
        public void GetPerson_NotNull()
        {
            var id = 1;

            var result = _service.GetPerson(id);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetPerson_ThrowsException()
        {
            long id = 0;
            var exception = Assert.Throws<Exception>(() => _service.GetPerson(id));

            Assert.True(exception.Message == "Invalid Entity key");
            _appInsightsService.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        [Fact]
        public void MapFromDto_NotNull_IsType()
        {
            var person = GetPerson(1);

            var dto = _mapper.Map<PersonDto>(person);

            Assert.NotNull(dto);
            Assert.IsType<PersonDto>(dto);
        }

        [Fact]
        public void Validate_True()
        {
            var person = GetPerson(1);

            var result = _service.Validate(person);

            Assert.True(result.IsValid);
        }

        #region Mocking
        private Mock<IUnitOfWork> GetMockUow()
        {
            var mock = new Mock<IUnitOfWork>();
          
            mock.Setup(x => x.Person.GetById(It.Is<long>(keyId => keyId > 0))).Returns((long id) => GetPerson(id));
            mock.Setup(x => x.Person.GetById(It.Is<long>(keyId => keyId == 0))).Throws(new Exception("Invalid Entity key"));

            return mock;
        }

        

        private IEnumerable<PersonDto> GetPersonsDto(int quantity) 
        {
            List<PersonDto> persons = new List<PersonDto>();

            for(int i = 0; i < quantity; i++)
            {
                var id = i + 1;

                persons.Add(new PersonDto
                {
                    Id = i + 1,
                    DateBirth = DateTime.Now.AddYears(i - 18),
                    FirstName = "Foo",
                    LastName = $"Bar {id}",
                    PersonType = Domain.Enumerables.PersonTypeEnum.Employee
                });
            }

            return persons;
        }

        private PagedResult<Person> GetPagedResult(int quantity)
        {
            var persons = new List<Person>();

            for(int i = 0; i < quantity; i++)
            {
                var id = i + 1;

                persons.Add(new Person
                {
                    Id = i + 1,
                    DateBirth = DateTime.Now.AddYears(i - 18),
                    CreatedAt = DateTime.Now,
                    Enabled = true,
                    FirstName = $"Foo {id}",
                    LastName = $"Bar {id}",
                    PersonType = Domain.Enumerables.PersonTypeEnum.Employee
                });
            }

            return new PagedResult<Person> 
            { 
                Queryable = persons.AsQueryable(),
                CurrentPage = 1,
                PageCount = 20,
                PageSize = 20,
                RowCount = persons.Count()
            };
        }

        private Person GetPerson(long id) =>
            new Person 
            { 
                Id = id,
                Age = 30,
                DateBirth = DateTime.Now.AddYears(-30),
                CreatedAt = DateTime.Now,
                Enabled = true,
                FirstName = $"Foo",
                LastName = $"Bar {id}",
                PersonType = Domain.Enumerables.PersonTypeEnum.Employee,
                UpdatedAt = DateTime.Now
            };

        #endregion
    }
}
