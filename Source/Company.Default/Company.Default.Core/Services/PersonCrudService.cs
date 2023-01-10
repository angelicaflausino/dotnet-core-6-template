using Company.Default.Domain.Entities;
using Company.Default.Domain.Services;
using Company.Default.Infra.Base;
using Microsoft.Extensions.Logging;

namespace Company.Default.Core.Services
{
    public class PersonCrudService : ICrudService<Person>
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<PersonCrudService> _logger;

        public PersonCrudService(IUnitOfWork uow, ILoggerFactory loggerFactory)
        {
            _uow = uow;
            _logger = loggerFactory.CreateLogger<PersonCrudService>();
        }

        public Person Create(Person entity)
        {
            try
            {
                _uow.BeginTransaction();
                _uow.Person.Add(entity);
                _uow.SaveChanges();
                _uow.CommitTransaction();

                return entity;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                _uow.RollbackTransaction();
                throw;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var entity = Get(id);

                _uow.BeginTransaction();
                _uow.Person.Delete(entity);
                _uow.SaveChanges();
                _uow.CommitTransaction();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                _uow.RollbackTransaction();
                throw;
            }
        }

        public Person Get(int id)
        {
            try
            {
                return _uow.Person.GetById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public void Update(Person entity)
        {
            try
            {
                _uow.BeginTransaction();
                _uow.Person.Update(entity);
                _uow.SaveChanges();
                _uow.CommitTransaction();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                _uow.RollbackTransaction();
                throw;
            }
        }
    }
}
