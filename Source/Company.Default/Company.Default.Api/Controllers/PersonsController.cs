using Company.Default.Api.Scopes;
using Company.Default.Domain.Dtos;
using Company.Default.Domain.Entities;
using Company.Default.Domain.Filters;
using Company.Default.Domain.Services;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace Company.Default.Api.Controllers
{
    /// <summary>
    /// Sample of controller for entity domain
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonService _service;
        private readonly ICrudService<Person, long> _crudService;
     
        public PersonsController(IPersonService service, ICrudService<Person, long> crudService)
        {
            _service = service; 
            _crudService = crudService;
        }

        /// <summary>
        /// Get Person by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [RequiredScopeOrAppPermission(
            AcceptedScope = new[] { ApiScopes.PERSON_READ },
            AcceptedAppPermission = new[] { ApiScopes.PERSON_READ_ALL })]
        [ProducesResponseType(typeof(PersonDto), 200)]
        public IActionResult Get([FromRoute]long id)
        {
            var result = _service.GetPerson(id);

            return Ok(result);
        }

        /// <summary>
        /// Get paged search
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("search")]
        [RequiredScopeOrAppPermission(
            AcceptedScope = new[] { ApiScopes.PERSON_READ },
            AcceptedAppPermission = new[] { ApiScopes.PERSON_READ_ALL })]
        [ProducesResponseType(typeof(PagedResultDto<PersonDto>), 200)]
        public IActionResult GetSearch([FromQuery]PersonFilterParameter filter) 
        {
            var result = _service.GetPagedSearch(filter);

            return Ok(result);
        }

        /// <summary>
        /// Get all Persons
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [RequiredScopeOrAppPermission(
            AcceptedScope = new[] { ApiScopes.PERSON_READ },
            AcceptedAppPermission = new[] { ApiScopes.PERSON_READ_ALL })]
        [ProducesResponseType(typeof(IEnumerable<PersonDto>), 200)]
        public IActionResult GetAll()
        {
            var result = _service.GetAll();

            return Ok(result);
        }

        /// <summary>
        /// Create Person
        /// </summary>
        /// <param name="personDto"></param>
        /// <returns></returns>
        [HttpPost]
        [RequiredScopeOrAppPermission(
            AcceptedScope = new[] { ApiScopes.PERSON_READ_WRITE },
            AcceptedAppPermission = new[] { ApiScopes.PERSON_READ_WRITE_ALL })]
        [ProducesResponseType(typeof(Person), 200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        public IActionResult Post([FromBody]PersonDto personDto)
        {
            var person = _service.MapFromDto(personDto);
            var validate = _service.Validate(person);

            if (!validate.IsValid)
                return BadRequest(validate);

            var result = _crudService.Create(person);

            return Ok(result);
        }

        /// <summary>
        /// Update Person
        /// </summary>
        /// <param name="personDto"></param>
        /// <returns></returns>
        [HttpPut]
        [RequiredScopeOrAppPermission(
            AcceptedScope = new[] { ApiScopes.PERSON_READ_WRITE },
            AcceptedAppPermission = new[] { ApiScopes.PERSON_READ_WRITE_ALL })]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        public IActionResult Put([FromBody] PersonDto personDto)
        {
            var person = _service.MapFromDto(personDto);
            var validate = _service.Validate(person);

            if (!validate.IsValid)
                return BadRequest(validate);

            _crudService.Update(person);

            return NoContent();
        }

        /// <summary>
        /// Delete Person
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [RequiredScopeOrAppPermission(
            AcceptedScope = new[] { ApiScopes.PERSON_DELETE },
            AcceptedAppPermission = new[] { ApiScopes.PERSON_DELETE_ALL })]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        public IActionResult Delete([FromRoute] long id)
        {
            var result = _crudService.Delete(id);

            return Ok(result);
        }
    }
}
