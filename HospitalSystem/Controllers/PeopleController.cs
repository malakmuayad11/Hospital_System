using HospitalSystem.API.Validation;
using HospitalSystem.DTOs;
using HospitalSystem.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    [Route("api/people")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PeopleController(IPersonService personService) => this._personService = personService;

        [HttpPost(Name = "AddNewPerson")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<bool>> AddNewPersonAsync(AddPersonDto addPersonDto)
        {
            if (!PersonValidation.validateAddPersonDto(addPersonDto))
                return BadRequest("Please validate your input");

            int? personId = await _personService.AddNewPersonAsync(addPersonDto);

            if(personId == null)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while adding the new person." });

            return Ok(personId); // change to the getPerson route
        }

        [HttpGet("{personId}", Name = "FindById")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType (StatusCodes.Status200OK)]
        public async Task<ActionResult<PersonDto>> FindAsync(int personId)
        {
            if (!PersonValidation.validatePersonId(personId))
                return BadRequest("Please validate your input.");

            PersonDto personDto = await _personService.FindAsync(personId);

            if (personDto == null)
                return NotFound($"Person with id: {personId} is not found.");

            return Ok(personDto);
        }

        [HttpPut(Name = "UpdatePerson")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool?>> updateAsync(UpdatePersonDto updatePersonDto)
        {
            if (!PersonValidation.validateUpdatePersonDto(updatePersonDto))
                return BadRequest("Please validate your input.");

            bool? result = await _personService.updateAsync(updatePersonDto);

            if (result == null)
                return NotFound($"Person with id: {updatePersonDto.PersonId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while updating the person." });

            return Ok("Person is updated successfully.");
        }
    }
}
