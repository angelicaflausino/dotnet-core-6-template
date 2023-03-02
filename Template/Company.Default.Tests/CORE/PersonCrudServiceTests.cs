using $ext_safeprojectname$.Cloud.Interfaces;
using $ext_safeprojectname$.Core.Services;
using $ext_safeprojectname$.Domain.Contracts.Repositories;
using $ext_safeprojectname$.Domain.Entities;
using System.Data.SqlTypes;

namespace $safeprojectname$.Core
{
    public class PersonCrudServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IAppInsightsService> _mockAppInsightsService;
        private readonly PersonCrudService _service;

        public PersonCrudServiceTests()
        {
            _mockUow = GetMockUoW();
            _mockAppInsightsService = MockingUtils.GetMockAppInsightsService();
            _service = new PersonCrudService(_mockUow.Object, _mockAppInsightsService.Object);
        }

        [Fact]
        public void Create_NotNull_Verify()
        {
            var person = GeneratePerson(0);

            var result = _service.Create(person);

            Assert.NotNull(result);
            _mockUow.Verify(x => x.BeginTransaction(), Times.Once);
            _mockUow.Verify(x => x.Person.Add(It.Is<Person>(p => p.Equals(person))), Times.Once());
            _mockUow.Verify(x => x.SaveChanges(), Times.Once);
            _mockUow.Verify(x => x.CommitTransaction(), Times.Once);
        }

        [Fact]
        public void Create_ThrowsException()
        {
            var person = new Person();

            var exception = Assert.Throws<SqlNullValueException>(() => _service.Create(person));

            Assert.IsType<SqlNullValueException>(exception);
            _mockUow.Verify(x => x.RollbackTransaction(), Times.Once);
            _mockAppInsightsService.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public void Get_NotNull()
        {
            long id = 1;

            var result = _service.Get(id);

            Assert.NotNull(result);
            _mockUow.Verify(x => x.Person.GetById(It.Is<long>(keyId => keyId == id)), Times.Once());
        }

        [Fact]
        public void Get_Null()
        {
            long id = 0;

            var result = _service.Get(id);

            Assert.Null(result);
            _mockUow.Verify(x => x.Person.GetById(It.Is<long>(keyId => keyId == id)), Times.Once);
        }

        [Fact]
        public void Delete_True()
        {
            long id = 1;

            var result = _service.Delete(id);

            Assert.True(result);
            _mockUow.Verify(x => x.Person.GetById(It.Is<long>(keyId => keyId == id)), Times.Once());
            _mockUow.Verify(x => x.BeginTransaction(), Times.Once());
            _mockUow.Verify(x => x.SaveChanges(), Times.Once);
            _mockUow.Verify(x => x.CommitTransaction(), Times.Once);
        }

        [Fact]
        public void Delete_ThrowsException()
        {
            long id = 0;

            var exception = Assert.Throws<NullReferenceException>(() => _service.Delete(id));

            Assert.IsType<NullReferenceException>(exception);
            _mockUow.Verify(x => x.Person.GetById(It.Is<long>(keyId => keyId == id)), Times.Once());
            _mockUow.Verify(x => x.BeginTransaction(), Times.Once);
            _mockUow.Verify(x => x.RollbackTransaction(), Times.Once());
            _mockUow.Verify(x => x.SaveChanges(), Times.Never());
            _mockUow.Verify(x => x.CommitTransaction(), Times.Never());
            _mockAppInsightsService.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public void Update_Verify()
        {
            var person = GeneratePerson(1);
            person.UpdatedAt = DateTime.Now;

            _service.Update(person);

            _mockUow.Verify(x => x.BeginTransaction(), Times.Once);
            _mockUow.Verify(x => x.Person.Update(It.Is<Person>(p => p == person)), Times.Once);
            _mockUow.Verify(x => x.SaveChanges(), Times.Once);
            _mockUow.Verify(x => x.CommitTransaction(), Times.Once);
        }

        [Fact]
        public void Update_ThrowsException()
        {
            var person = new Person();

            var exception = Assert.Throws<NullReferenceException>(() => _service.Update(person));

            Assert.IsType<NullReferenceException>(exception);
            _mockUow.Verify(x => x.BeginTransaction(), Times.Once);
            _mockUow.Verify(x => x.RollbackTransaction(), Times.Once());
            _mockUow.Verify(x => x.SaveChanges(), Times.Never());
            _mockUow.Verify(x => x.CommitTransaction(), Times.Never());
            _mockAppInsightsService.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }

        #region Mocking
        private Mock<IUnitOfWork> GetMockUoW()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.SaveChanges()).Verifiable();
            mock.Setup(x => x.CommitTransaction()).Verifiable();
            mock.Setup(x => x.RollbackTransaction()).Verifiable();
            mock.Setup(x => x.BeginTransaction()).Verifiable();
            mock.Setup(x => x.Person).Returns(MockPersonRepository().Object);

            return mock;
        }

        private Mock<IPersonRepository> MockPersonRepository()
        {
            var mock = new Mock<IPersonRepository>();
            Person nullPerson = null;

            mock.Setup(x => x.Add(It.Is<Person>(person => IsValidPerson(person)))).Verifiable();
            mock.Setup(x => x.Add(It.Is<Person>(person => !IsValidPerson(person)))).Throws<SqlNullValueException>();
            mock.Setup(x => x.GetById(It.Is<long>(id => id > 0))).Returns(GeneratePerson(1));
            mock.Setup(x => x.GetById(It.Is<long>(id => id == 0))).Returns(nullPerson);
            mock.Setup(x => x.Delete(It.Is<Person>(person => IsValidPerson(person)))).Verifiable();
            mock.Setup(x => x.Delete(It.Is<Person>(person => !IsValidPerson(person)))).Throws<NullReferenceException> ();
            mock.Setup(x => x.Update(It.Is<Person>(person => IsValidPerson(person)))).Verifiable();
            mock.Setup(x => x.Update(It.Is<Person>(person => !IsValidPerson(person)))).Throws<NullReferenceException>();
            return mock;
        }

        private Person GeneratePerson(long id) => new Person
        {
            Id = id,
            CreatedAt = DateTime.Now,
            DateBirth = DateTime.Now.AddYears(-18),
            Enabled = true,
            FirstName = "Test",
            LastName = "Test",
            PersonType = Domain.Enumerables.PersonTypeEnum.Employee
        };

        private bool IsValidPerson(Person person) =>
            !string.IsNullOrEmpty(person.FirstName) &&
            !string.IsNullOrEmpty(person.LastName) &&
            person.DateBirth != DateTime.MinValue &&
            person.CreatedAt != DateTime.MinValue;
        #endregion
    }
}
