using $ext_safeprojectname$.Domain.Contracts.Repositories;
using $ext_safeprojectname$.Domain.Entities;
using $safeprojectname$.Base;
using $safeprojectname$.Contexts;

namespace $safeprojectname$.Repositories
{
    public class PersonRepository : RepositoryBase<Person, long>, IPersonRepository
    {
        public PersonRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Overloaded for logical deletion
        /// </summary>
        /// <param name="entity"></param>
        public override void Delete(Person entity)
        {
            entity.UpdatedAt = DateTime.Now;
            entity.Enabled = false;
            this.Update(entity);
        }

        /// <summary>
        /// Gets all records containing first name.
        /// Represent custom method from specialized class
        /// </summary>
        /// <param name="firstName"></param>
        /// <returns></returns>
        public IEnumerable<Person> GetAllByName(string firstName)
        {
            return this.GetAll(x => x.FirstName.Contains(firstName));
        }
    }
}
