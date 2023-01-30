using AutoMapper;
using Company.Default.Domain.Base;
using Company.Default.Domain.Dtos;
using Company.Default.Domain.Entities;
using Company.Default.Domain.Filters;
using Company.Default.Domain.Services;
using Company.Default.Infra.Base;
using FluentValidation;
using FluentValidation.Results;
using LinqKit;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Company.Default.Core.Services
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<PersonService> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<Person> _validator;

        public PersonService(IUnitOfWork uow, ILogger<PersonService> logger, IMapper mapper, IValidator<Person> validator)
        {
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
            _validator = validator;
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
                _logger.LogError(ex.Message, ex);
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

                //TODO: Seria melhor projetar o dto com o linq
                return new PagedResultDto<PersonDto> 
                { 
                    Total = paged.RowCount, 
                    Result = _mapper.Map<IList<PersonDto>>(persons) 
                };              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        //TODO: Projetar o Dto?
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
                _logger.LogError(ex.Message, ex);
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
            Age = y.Age,
            FirstName = y.FirstName,
            LastName = y.LastName,
            FullName = y.FirstName + " " + y.LastName,
            PersonType = y.PersonType
        };
        #endregion

    }
}
