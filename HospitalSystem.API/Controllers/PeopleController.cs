using HospitalSystem.API.Validation;
using HospitalSystem.Infrastructure.DTOs.People;
using HospitalSystem.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace HospitalSystem.API.Controllers
{
    [Authorize]
    [Route("api/people")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly ILoggerService _loggerService;
        public PeopleController(IPersonService personService, ILoggerService loggerService)
        {
            _personService = personService;
            _loggerService = loggerService;
        }

        [EnableRateLimiting("CriticalOpsLimiter")]
        [HttpPost(Name = "AddNewPerson")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<AddPersonDto>> AddNewPersonAsync(AddPersonDto addPersonDto)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";

            if (!PersonValidation.validateAddPersonDto(addPersonDto))
                return BadRequest("Please validate your input");

            int? personId = await _personService.AddNewPersonAsync(addPersonDto);

            if(personId is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while adding the new person." });

            _loggerService.Log($"Added person with id: {personId}.", ip, userId);
            return CreatedAtRoute("FindById", new { personId = personId }, addPersonDto);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("{personId}", Name = "FindById")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType (StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<PersonDto>> FindAsync(int personId)
        {
            if (!PersonValidation.validatePersonId(personId))
                return BadRequest("Please validate your input.");

            PersonDto personDto = await _personService.FindAsync(personId);

            if (personDto == null)
                return NotFound($"Person with id: {personId} is not found.");

            return Ok(personDto);
        }

        [EnableRateLimiting("CriticalOpsLimiter")]
        [HttpPut(Name = "UpdatePerson")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<bool?>> updateAsync(UpdatePersonDto updatePersonDto)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";

            if (!PersonValidation.validateUpdatePersonDto(updatePersonDto))
                return BadRequest("Please validate your input.");

            bool? result = await _personService.UpdateAsync(updatePersonDto);

            if (result is null)
                return NotFound($"Person with id: {updatePersonDto.PersonId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while updating the person." });

            _loggerService.Log($"Updated person with id: {updatePersonDto.PersonId}.", ip, userId);
            return Ok("Person is updated successfully.");
        }
    }
}
