using Company.Default.Domain.Contracts.Repositories;
using Company.Default.Infra.Contexts;
using Company.Default.Infra.Repositories;

namespace Company.Default.Infra.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IPersonRepository _personRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public void BeginTransaction() => _context.Database.BeginTransaction();

        public void CommitTransaction() => _context.SaveChanges();

        public void RollbackTransaction() => _context.Database.RollbackTransaction();

        public void SaveChanges() => _context.SaveChanges();

        public IPersonRepository Person => _personRepository = _personRepository ?? new PersonRepository(_context);
    }
}
