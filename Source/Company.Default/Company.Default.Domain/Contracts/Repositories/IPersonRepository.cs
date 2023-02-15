using Company.Default.Domain.Entities;

namespace Company.Default.Domain.Contracts.Repositories
{
    public interface IPersonRepository : IRepository<Person, long>
    {
        IEnumerable<Person> GetAllByName(string firstName);
    }
}
