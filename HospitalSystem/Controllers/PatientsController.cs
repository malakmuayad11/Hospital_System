using HospitalSystem.API.Validation;
using HospitalSystem.Infrastructure.DTOs.Patients;
using HospitalSystem.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace HospitalSystem.API.Controllers
{
    [Authorize(Policy = "ManagePatients")]
    [Route("api/patients")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly ILoggerService _loggerService;
        public PatientsController(IPatientService patientService, ILoggerService loggerService)
        {
            _patientService = patientService;
            _loggerService = loggerService;
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetAllPatientsAsync()
        {
            List<PatientDto> patientDtos = await _patientService.GetAllPatientsAsync();

            if (patientDtos is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while fetching patients." });

            return Ok(patientDtos);
        }

        [EnableRateLimiting("CriticalOpsLimiter")]
        [HttpPost(Name = "AddNewPatient")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<PatientDto>> AddNewPatientAsync(AddPatientDto addPatientDto)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";

            if (!PatientValidation.ValidateAddPatientDto(addPatientDto))
                return BadRequest("Please validate your input.");

            int? patientId = await _patientService.AddNewPatientAsync(addPatientDto);

            if(patientId is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while adding the new patient." });

            _loggerService.Log($"Added patient with id: {patientId}.", ip, userId);
            return CreatedAtRoute("GetPatientByPatientId", new { patientId = patientId }, addPatientDto);
        }

        [EnableRateLimiting("CriticalOpsLimiter")]
        [HttpPut(Name = "UpdatePatient")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult> UpdatePatientAsync(UpdatePatientDto updatePatientDto)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";

            if (!PatientValidation.ValidateUpdatePatientDto(updatePatientDto))
                return BadRequest("Please validate your input");

            bool? result = await _patientService.UpdatePatientAsync(updatePatientDto);

            if(result is null)
                return NotFound($"Patient with id: {updatePatientDto.PatientId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while updating the patient." });

            _loggerService.Log($"Updated patient with id: {updatePatientDto.PatientId}.", ip, userId);
            return Ok("Patient is updated successfully.");
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("by-id/{patientId}", Name = "GetPatientByPatientId")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<PatientDto>> FindAsync(int patientId)
        {
            if (!PatientValidation.ValidatePatientId(patientId))
                return BadRequest("Please validate your input.");

            PatientDto patientDto = await _patientService.FindAsync(patientId);

            if (patientDto is null)
                return NotFound($"Patient with patientId: {patientId} is not found.");

            return Ok(patientDto);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("by-national/{nationalNo}", Name = "GetPatientByNationalNo")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<PatientDto>> FindAsync(string nationalNo)
        {
            if (!PatientValidation.ValidateNationalNo(nationalNo))
                return BadRequest("Please validate your input.");

            PatientDto patientDto = await _patientService.FindAsync(nationalNo);

            if(patientDto is null)
                return NotFound($"Patient with nationalNo: {nationalNo} is not found.");

            return Ok(patientDto);
        }

        [EnableRateLimiting("CriticalOpsLimiter")]
        [HttpDelete("{patientId}", Name = "DeletePatient")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<bool>> DeleteAsync(int patientId)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";

            if (!PatientValidation.ValidatePatientId(patientId))
                return BadRequest("Please validate your input.");

            bool? result = await _patientService.DeletePatientAsync(patientId);

            if (result is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while deleting the patient." });

            if (result == false)
                    return NotFound($"Patient with id: {patientId} is not found.");

            _loggerService.Log($"Deleted patient with id: {patientId}.", ip, userId);
            return Ok(result);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("national-no/{nationalNo}/exists", Name = "DoesNationalNoExist")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<bool>> DoesNationalNoExistAsync(string nationalNo)
        {
            if (!PatientValidation.ValidateNationalNo(nationalNo))
                return BadRequest("Please validate your input.");

            return Ok(await _patientService.DoesNationalNoExistAsync(nationalNo));
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("medical-records/{patientId}/exists", Name = "HasPatientMedicalRecords")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<bool>> HasPatientMedicalRecordsAsync(int patientId)
        {
            if (!PatientValidation.ValidatePatientId(patientId))
                return BadRequest("Please validate your input.");

            bool? result = await _patientService.HasPatientMedicalRecordsAsync(patientId);

            if(result is null)
                return NotFound($"Patient with id: {patientId} is not found.");

            return Ok(result);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("prescriptions/{patientId}/exists", Name = "HasPatientPrescriptions")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<bool>> HasPatientPrescriptionsAsync(int patientId)
        {
            if (!PatientValidation.ValidatePatientId(patientId))
                return BadRequest("Please validate your input.");

            bool? result = await _patientService.HasPatientPrescriptionsAsync(patientId);

            if (result is null)
                return NotFound($"Patient with id: {patientId} is not found.");

            return Ok(result);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("appointments/{patientId}/{appointmentDate}/{appointmentTime}/exists", Name = "HasPatientAppointmentAt")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<bool>> HasPatientAppointmentAtAsync(int patientId, 
            DateOnly appointmentDate, TimeOnly appointmentTime)
        {
            if (!PatientValidation.ValidatePatientId(patientId))
                return BadRequest("Please validate your input.");

            bool? result = await _patientService.HasPatientAppointmentAtAsync(patientId,
                appointmentDate, appointmentTime);

            if (result is null)
                return NotFound($"Patient with id: {patientId} is not found.");

            return Ok(result);
        }
    }
}
