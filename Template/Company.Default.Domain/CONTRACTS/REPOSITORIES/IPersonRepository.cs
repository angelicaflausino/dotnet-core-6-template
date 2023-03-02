using $safeprojectname$.Entities;

namespace $safeprojectname$.Contracts.Repositories
{
    public interface IPersonRepository : IRepository<Person, long>
    {
        IEnumerable<Person> GetAllByName(string firstName);
    }
}
