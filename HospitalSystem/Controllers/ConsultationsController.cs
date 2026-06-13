using HospitalSystem.Infrastructure.DTOs.Consultations;
using HospitalSystem.Service.Interfaces;
using HospitalSystem.Service.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HospitalSystem.API.Controllers
{
    [EnableRateLimiting("LightOpsLimiter")]
    [Authorize]
    [Route("api/consultations")]
    [ApiController]
    public class ConsultationsController : ControllerBase
    {
        private readonly IConsultationService _consultationService;

        public ConsultationsController(IConsultationService consultationService) =>
            _consultationService = consultationService;

        [HttpGet(Name = "GetAllConsultations")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<IEnumerable<ConsultationDto>>> GetAllConsultations()
        {
            List<ConsultationDto> consultations = await _consultationService.GetAllConsultationsAsync();
            
            if(consultations is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
           new { message = "An error occurred while getting consultations." });
            
            return Ok(consultations);
        }

        [HttpGet("{consultationId}", Name = "FindConsultation")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<FindConsultationDto>> Find(int consultationId)
        {
            if (!ConsultationValidation.ValidateConsultationId(consultationId))
                return BadRequest("Please validate your input.");

            FindConsultationDto findConsultationDto = await _consultationService.FindAsync(consultationId);

            if (findConsultationDto is null)
                return NotFound($"Consultation with id: {consultationId} is not found.");

            return Ok(findConsultationDto);
        }

        [HttpGet("specialities", Name = "GetAllSpecialities")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<IEnumerable<ConsultationDto>>> GetAllSpecialities()
        {
            List<string> specialities = await _consultationService.GetAllSpecialitiesAsync();

            if (specialities is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
           new { message = "An error occurred while getting specialities." });

            return Ok(specialities);
        }
    }
}
