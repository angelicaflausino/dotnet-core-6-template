using Company.Default.Domain.Dtos;
using Company.Default.Domain.Entities;
using Company.Default.Domain.Filters;
using Company.Default.Domain.Services;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Company.Default.Api.Controllers
{
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
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        public IActionResult Delete([FromRoute] long id)
        {
            var result = _crudService.Delete(id);

            return Ok(result);
        }
    }
}
