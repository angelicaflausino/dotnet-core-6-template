using Company.Default.Domain.Base;
using Company.Default.Domain.Dtos;
using Company.Default.Domain.Entities;
using Company.Default.Domain.Filters;

namespace Company.Default.Domain.Services
{
    public interface IPersonService
    {
        PagedResultDto<PersonDto> GetPagedSearch(PersonFilterParameter parameter);
        PersonDto GetPerson(long id);
        IEnumerable<PersonDto> GetAll();
        Person MapFromDto(PersonDto personDto);
        ValidatorResult Validate(Person person, params string[] rules);
    }
}
