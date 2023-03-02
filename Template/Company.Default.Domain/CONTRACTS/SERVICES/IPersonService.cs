using $safeprojectname$.Contracts.Base;
using $safeprojectname$.Dtos;
using $safeprojectname$.Entities;
using $safeprojectname$.Filters;

namespace $safeprojectname$.Contracts.Services
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
