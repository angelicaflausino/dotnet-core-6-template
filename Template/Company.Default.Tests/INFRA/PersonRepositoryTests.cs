using $ext_safeprojectname$.Domain.Dtos;
using $ext_safeprojectname$.Domain.Entities;
using $ext_safeprojectname$.Domain.Enumerables;
using $ext_safeprojectname$.Infra.Contexts;
using $ext_safeprojectname$.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace $safeprojectname$.Infra
{
    public class PersonRepositoryTests
    {
        private readonly PersonRepository _repository;
        private static AppDbContext _context;
        private static int _globalId = 0;

        public PersonRepositoryTests()
        {
            _context = GetAppDbContext();
            _repository = GetPersonRepository();
        }

        [Fact]
        public void Add_NotNull()
        {
            var person = GeneratePerson();
            
            _repository.Add(person);
            _context.SaveChanges();
            var result = _context.Set<Person>().Find(person.Id);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddAsync_NotNull()
        {
            var person = GeneratePerson();

            await _repository.AddAsync(person);
            await _context.SaveChangesAsync();
            var result = _context.Set<Person>().Find(person.Id); 
            
            Assert.NotNull(result);
        }

        [Fact]
        public void AddRange_True()
        {
            var persons = GeneratePersonList(5);
            var ids = persons.Select(x => x.Id);

            _repository.AddRange(persons);
            _context.SaveChanges();
            var result = _context.Set<Person>().All(x => ids.Contains(x.Id));

            Assert.True(result);
        }

        [Fact]
        public async Task AddRangeAsync_True()
        {
            var persons = GeneratePersonList(5);
            var ids = persons.Select(x => x.Id);

            await _repository.AddRangeAsync(persons);
            await _context.SaveChangesAsync();
            var result = _context.Set<Person>().All(x => ids.Contains(x.Id));

            Assert.True(result);
        }

        [Fact]
        public void GetById_NotNull()
        {
            var person = GenerateAndSavePerson();

            var result = _repository.GetById(person.Id);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByIdAsync_NotNull()
        {
            var person = GenerateAndSavePerson();

            var result = await _repository.GetByIdAsync(person.Id);

            Assert.NotNull(result);
        }

        [Fact]
        public void FindById_NotNull()
        {
            var person = GenerateAndSavePerson();

            var result = _repository.FindById(person.Id);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task FindByIdAsync_NotNull()
        {
            var person = GenerateAndSavePerson();

            var result = await _repository.FindByIdAsync(person.Id);

            Assert.NotNull(result);
        }

        [Fact]
        public void Update_True()
        {
            var person = GenerateAndSavePerson();
            person.UpdatedAt = DateTime.Now;

            _repository.Update(person);
            _context.SaveChanges();

            var result = _context.Set<Person>().Find(person.Id);

            Assert.True(result.UpdatedAt.HasValue);
        }

        [Fact]
        public void UpdateRange_True()
        {
            var persons = GenerateAndSavePersonList(5);
            persons.ForEach(x => x.UpdatedAt = DateTime.Now);
            var ids = persons.Select(x => x.Id).ToList();

            _repository.UpdateRange(persons);
            _context.SaveChanges();
            var result = _context.Set<Person>().Where(x => ids.Contains(x.Id)).ToList();

            Assert.True(result.All(x => x.UpdatedAt.HasValue));
        }

        [Fact]
        public void Delete_True()
        {
            var person = GenerateAndSavePerson();

            _repository.Delete(person);
            _context.SaveChanges();
            var result = _context.Set<Person>().Any(x => x.Id == person.Id && !x.Enabled);

            Assert.True(result);            
        }

        [Fact]
        public void DeleteRange_True()
        {
            var persons = GenerateAndSavePersonList(2);
            var ids = persons.Select(x => x.Id).ToList();

            _repository.DeleteRange(persons);
            _context.SaveChanges();
            var result = _context.Set<Person>().All(x => ids.Contains(x.Id) && !x.Enabled);

            Assert.True(result);
        }

        [Fact]
        public void GetAll_NotEmpty()
        {
            var persons = GenerateAndSavePersonList(5);

            var result = _repository.GetAll();

            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetAllExpression_NotEmpty()
        {
            var persons = GenerateAndSavePersonList(3);
            var ids = persons.Select(x => x.Id).ToList();

            var result = _repository.GetAll(x => ids.Contains(x.Id));

            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetSelect_NotEmpty()
        {
            var persons = GenerateAndSavePersonList(3);
            var ids = persons.Select(x => x.Id).ToList();

            var result = _repository.GetSelect(x => ids.Contains(x.Id), x => new PersonDto { Id = x.Id });

            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetPaged_NotNull_True()
        {
            var persons = GenerateAndSavePersonList(3);
            var ids = persons.Select(x => x.Id).ToList();

            var result = _repository.GetPaged(x => ids.Contains(x.Id), 1, 10, "FirstName asc");

            Assert.NotNull(result);
            Assert.True(result.RowCount == 3);
        }

        [Fact]
        public void GetQueryable_NotNull()
        {
            var person = GenerateAndSavePerson();

            var queryable = _repository.GetQueryable();
            var result = queryable.FirstOrDefault(x => x.Id == person.Id);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetAllByName_NotEmpty()
        {
            var persons = GenerateAndSavePersonList(3);

            var result = _repository.GetAllByName("Foo");

            Assert.NotEmpty(result);
        }


        #region Privates
        public static AppDbContext GetAppDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private PersonRepository GetPersonRepository()
        {
            var repository = new PersonRepository(_context);

            return repository;
        }

        private Person GeneratePerson()
        {
            var random = new Random();
            var age = random.Next(18, 60);
            var values = Enum.GetValues(typeof(PersonTypeEnum));
            var personType = (PersonTypeEnum)values.GetValue(random.Next(values.Length));

            var person = new Person
            {
                Id = ++_globalId,
                Age = age,
                DateBirth = DateTime.Now.AddYears(-age),
                CreatedAt = DateTime.Now,
                Enabled = true,
                FirstName = "Foo",
                LastName = $"Bar {_globalId}",
                PersonType = personType                
            };  

            return person;
        }

        private List<Person> GeneratePersonList(int quantity)
        {
            var list = new List<Person>();

            for(int i = 0; i < quantity; i++)
            {
                list.Add(GeneratePerson());
            }

            return list;
        }

        private Person GenerateAndSavePerson()
        {
            var person = GeneratePerson();

            _repository.Add(person);
            _context.SaveChanges();

            return person;
        }

        private List<Person> GenerateAndSavePersonList(int quantity)
        {
            var list = GeneratePersonList(quantity);

            _repository.AddRange(list);
            _context.SaveChanges();

            return list;
        }
        #endregion
    }
}
