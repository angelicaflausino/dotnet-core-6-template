using Company.Default.Cloud.Interfaces;
using Company.Default.Domain.Contracts.Repositories;
using Company.Default.Domain.Contracts.Services;
using Company.Default.Domain.Entities;

namespace Company.Default.Core.Services
{
    public class PersonCrudService : ICrudService<Person, long>
    {
        private readonly IUnitOfWork _uow;
        private readonly IAppInsightsService _appInsightsService;

        public PersonCrudService(IUnitOfWork uow, IAppInsightsService appInsightsService)
        {
            _uow = uow;
            _appInsightsService = appInsightsService;
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
                _appInsightsService.LogError(ex.Message, ex);
                _uow.RollbackTransaction();
                throw;
            }
        }

        public bool Delete(long id)
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
                _appInsightsService.LogError(ex.Message, ex);
                _uow.RollbackTransaction();
                throw;
            }
        }

        public Person Get(long id)
        {
            try
            {
                return _uow.Person.GetById(id);
            }
            catch (Exception ex)
            {
                _appInsightsService.LogError(ex.Message, ex);
                throw;
            }
        }

        public void Update(Person entity)
        {
            try
            {
                entity.UpdatedAt = DateTime.UtcNow;

                _uow.BeginTransaction();
                _uow.Person.Update(entity);
                _uow.SaveChanges();
                _uow.CommitTransaction();
            }
            catch (Exception ex)
            {
                _appInsightsService.LogError(ex.Message, ex);
                _uow.RollbackTransaction();
                throw;
            }
        }
    }
}
