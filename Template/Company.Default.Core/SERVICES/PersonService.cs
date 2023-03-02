using AutoMapper;
using $ext_safeprojectname$.Cloud.Interfaces;
using $ext_safeprojectname$.Domain.Contracts.Base;
using $ext_safeprojectname$.Domain.Contracts.Repositories;
using $ext_safeprojectname$.Domain.Contracts.Services;
using $ext_safeprojectname$.Domain.Dtos;
using $ext_safeprojectname$.Domain.Entities;
using $ext_safeprojectname$.Domain.Filters;
using FluentValidation;
using FluentValidation.Results;
using LinqKit;
using System.Linq.Expressions;

namespace $safeprojectname$.Services
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IValidator<Person> _validator;
        private readonly IAppInsightsService _appInsightsService;

        public PersonService(IUnitOfWork uow, IMapper mapper, IValidator<Person> validator, IAppInsightsService appInsightsService)
        {
            _uow = uow;
            _mapper = mapper;
            _validator = validator;
            _appInsightsService = appInsightsService;
        }

        public IEnumerable<PersonDto> GetAll()
        {
            try
            {
                var expression = GetDefaultExpression();
                var select = ProjectSelect();
                var result = _uow.Person.GetSelect(expression, select);

                return result;
            }
            catch (Exception ex)
            {
                _appInsightsService.LogError(ex.Message, ex);
                throw;
            }
        }

        public PagedResultDto<PersonDto> GetPagedSearch(PersonFilterParameter parameter)
        {
            try
            {
                var expression = GetSearchExpression(parameter);
                var paged = _uow.Person.GetPaged(expression, parameter.Page, parameter.Size, parameter.SortBy);
                var persons = paged.Queryable.ToList();

                return new PagedResultDto<PersonDto> 
                { 
                    Total = paged.RowCount, 
                    Result = _mapper.Map<IList<PersonDto>>(persons) 
                };              
            }
            catch (Exception ex)
            {
                _appInsightsService.LogError(ex.Message, ex);
                throw;
            }
        }

        public PersonDto GetPerson(long id)
        {
            try
            {
                var entity = _uow.Person.GetById(id);

                var result = _mapper.Map<PersonDto>(entity);

                return result;
            }
            catch (Exception ex)
            {
                _appInsightsService.LogError(ex.Message, ex);
                throw;
            }
        }

        public Person MapFromDto(PersonDto personDto) => _mapper.Map<Person>(personDto);

        public ValidatorResult Validate(Person person, params string[] rules)
        {
            
            var validator = _validator.Validate(person, opt =>
            {
                opt.IncludeRuleSets(rules);
            });

            return new ValidatorResult(validator.IsValid, GetValidatorErrors(validator.Errors));  
        }

        private IList<ValidatorError> GetValidatorErrors(IList<ValidationFailure> failures) =>
            failures.Select(x => new ValidatorError(x.PropertyName, x.ErrorMessage)).ToList();

        #region Expressions
        private Expression<Func<Person, bool>> GetSearchExpression(PersonFilterParameter filter)
        {
            var predicate = PredicateBuilder.New<Person>();

            predicate.Start(x => x.Enabled);

            if (filter.Type.HasValue)
                predicate.Or(x => x.PersonType == filter.Type.Value);

            if (!string.IsNullOrEmpty(filter.Name))
                predicate.Or(x => x.FirstName.Contains(filter.Name)).Or(x => x.LastName.Contains(filter.Name));

            if (filter.StartCreatedDate.HasValue && filter.EndCreatedDate.HasValue)
                predicate.Or(x => x.CreatedAt >= filter.StartCreatedDate.Value).Or(x => x.CreatedAt <= filter.EndCreatedDate.Value);

            if (filter.StartBirthDate.HasValue && filter.EndBirthDate.HasValue)
                predicate.Or(x => x.DateBirth >= filter.StartBirthDate.Value).Or(x => x.DateBirth <= filter.EndBirthDate.Value);

            if (filter.StartAge.HasValue && filter.EndAge.HasValue)
                predicate.Or(x => x.Age >= filter.StartAge.Value).Or(x => x.Age <= filter.EndAge.Value);

            return predicate;
        }

        private Expression<Func<Person, bool>> GetDefaultExpression() => PredicateBuilder.New<Person>().Start(x => x.Enabled);

        private Expression<Func<Person, PersonDto>> ProjectSelect() => y => new PersonDto
        {
            Id = y.Id,
            DateBirth = y.DateBirth,
            FirstName = y.FirstName,
            LastName = y.LastName,
            FullName = y.FirstName + " " + y.LastName,
            PersonType = y.PersonType
        };
        #endregion

    }
}
