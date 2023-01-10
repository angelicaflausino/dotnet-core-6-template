using Company.Default.Domain.Dtos;
using Company.Default.Domain.Filters;

namespace Company.Default.Domain.Services
{
    public interface IPersonService
    {
        PagedResultDto<PersonDto> GetPagedSearch(PersonFilterParameter parameter);
        PersonDto GetPerson(int id);
        IEnumerable<PersonDto> GetAll();
    }
}
