using HospitalSystem.API.Validation;
using HospitalSystem.Infrastructure.DTOs.MedicalRecords;
using HospitalSystem.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace HospitalSystem.API.Controllers
{
    [Route("api/medicalrecords")]
    [ApiController]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly ILoggerService _loggerService;

        public MedicalRecordsController(IMedicalRecordService mediicalRecordService, ILoggerService loggerService)
        {
            _medicalRecordService = mediicalRecordService;
            _loggerService = loggerService;
        }

        [EnableRateLimiting("CriticalOpsLimiter")]
        [Authorize(Policy = "AddEditMedicalRecords")]
        [HttpPost(Name = "AddNewMedicalRecord")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult> AddNewMedicalRecordAsync(AddMedicalRecordDto addMedicalRecordDto)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";

            if (!MedicalRecordValidation.ValidateAddMedicalRecordDto(addMedicalRecordDto))
                return BadRequest("Please validate your input");

            int? medicalRecordId = await _medicalRecordService.AddNewMedicalRecordAsync(addMedicalRecordDto);

            if (medicalRecordId is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while adding the new medical record." });

            _loggerService.Log($"Added medical record with id: {medicalRecordId}.", ip, userID);
            return CreatedAtRoute("FindMedicalRecordByID", new { medicalRecordId = medicalRecordId}, addMedicalRecordDto);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [Authorize(Policy = "ShowMedicalRecords")]
        [HttpGet(Name = "GetAllMedicalRecords")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<IEnumerable<MedicalRecordDto>>> GetAllMedicalRecordsAsync()
        {
            List<MedicalRecordDto> medicalRecords = await _medicalRecordService.GetAllMedicalRecordsAsync();

            if (medicalRecords is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while fetching medical records." });

            return Ok(medicalRecords);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [Authorize(Policy = "ShowMedicalRecords")]
        [HttpGet("{medicalRecordId}", Name = "FindMedicalRecordByID")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<MedicalRecordDto>> FindAsync(int medicalRecordId)
        {
            if (!MedicalRecordValidation.ValidateMedicalRecordId(medicalRecordId))
                return BadRequest("Please validate your input");

            MedicalRecordDto medicalRecord = await _medicalRecordService.FindAsync(medicalRecordId);

            if (medicalRecord is null)
                return NotFound(new { message = $"Medical record with ID {medicalRecordId} not found." });

            return Ok(medicalRecord);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [Authorize(Policy = "ShowMedicalRecords")]
        [HttpGet("appointment/{appointmentId}", Name = "FindMedicalRecordByAppointmentID")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<MedicalRecordDto>> FindByAppointmentIdAsync(int appointmentId)
        {
            if (!MedicalRecordValidation.ValidateAppointmentId(appointmentId))
                return BadRequest("Please validate your input");

            MedicalRecordDto medicalRecord = await _medicalRecordService.FindByAppointmentIdAsync(appointmentId);
            
            if (medicalRecord is null)
                return NotFound($"Medical record with appointment ID {appointmentId} not found.");

            return Ok(medicalRecord);
        }
    }
}
