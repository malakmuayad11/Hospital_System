using HospitalSystem.Infrastructure.DTOs.Appointments;
using HospitalSystem.Infrastructure.DTOs.Doctors;
using HospitalSystem.Service.Interfaces;
using HospitalSystem.Service.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace HospitalSystem.API.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILoggerService _loggerService;

        public DoctorsController(IDoctorService doctorService,
            IAuthorizationService authorizationService, ILoggerService loggerService)
        {
            _doctorService = doctorService;
            _authorizationService = authorizationService;
            _loggerService = loggerService;
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("count", Name = "GetDoctorsCount")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<int>> GetDoctorsCount()
        {
            int count = await _doctorService.DoctorsCount();

            if (count == null)
                return StatusCode(StatusCodes.Status500InternalServerError,
            new { message = "An error occurred while getting doctors count." });

            return Ok(count);
        }

        [EnableRateLimiting("CriticalOpsLimiter")]
        [Authorize(Policy = "AddEditDoctors")]
        [HttpPost(Name = "AddNewDoctor")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult> AddNewDoctor(AddDoctorDto addDoctorDto)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";

            if (!DoctorValidation.ValidateAddDoctorDto(addDoctorDto))
                return BadRequest("Please validate your input.");

            int? doctorId = await _doctorService.AddNewDoctorAsync(addDoctorDto);

            if (doctorId is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
            new { message = "An error occurred while adding new doctor." });

            _loggerService.Log($"Added doctor with doctorId {doctorId}", ip, userID);
            return CreatedAtRoute("FindDoctor", new { doctorId = doctorId }, new 
            {
                PersonId = addDoctorDto.PersonId,
                StartWorkDay = addDoctorDto.StartWorkDay,
                EndWorkDay = addDoctorDto.EndWorkDay,
                StartWorkHour = addDoctorDto.StartWorkHour,
                EndWorkHour = addDoctorDto.EndWorkHour,
                ConsultationId = addDoctorDto.ConsultationId
            });
        }

        [EnableRateLimiting("CriticalOpsLimiter")]
        [HttpPut(Name = "UpdateDoctor")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<bool>> UpdateDoctor(UpdateDoctorDto updateDoctorDto)
        {
            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(
                User,
                updateDoctorDto.UserId,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid();

            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";

            if (!DoctorValidation.ValidateUpdateDoctorDto(updateDoctorDto))
                return BadRequest("Please validate your input.");

            bool? result = await _doctorService.UpdateDoctorAsync(updateDoctorDto);

            if (result is null)
                return NotFound($"Doctor with id {updateDoctorDto.DoctorId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
            new { message = "An error occurred while updating doctor." });

            _loggerService.Log($"Updated doctor with id: {updateDoctorDto.DoctorId}", ip, userID);
            return Ok(result);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [Authorize(Policy = "AddEditDoctors")]
        [HttpGet("{doctorId}", Name = "FindDoctor")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<FindDoctorDto>> Find(int doctorId)
        {
            if (!DoctorValidation.ValidateDoctorId(doctorId))
                return BadRequest("Please validate your input.");

            FindDoctorDto doctorDto = await _doctorService.FindAsync(doctorId);

            if (doctorDto is null)
                return NotFound($"Doctor with id {doctorId} is not found.");

            return Ok(doctorDto);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("user/{userId}", Name = "FindDoctorByUserId")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<FindDoctorDto>> FindByUserId(int userId)
        {
            if (!DoctorValidation.ValidateUserId(userId))
                return BadRequest("Please validate your input.");

            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(
                User,
                userId,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); 

            FindDoctorDto doctorDto = await _doctorService.FindByUserIdAsync(userId);

            if (doctorDto is null)
                return NotFound($"Doctor with user id {userId} is not found.");

            return Ok(doctorDto);
        }

        [EnableRateLimiting("CriticalOpsLimiter")]
        [Authorize(Policy = "AddEditDoctors")]
        [HttpDelete("{doctorId}", Name = "DeleteDoctor")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<bool>> DeleteDoctor(int doctorId)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";

            if (!DoctorValidation.ValidateDoctorId(doctorId))
                return BadRequest("Please validate your input.");

            bool? result = await _doctorService.DeleteDoctorAsync(doctorId);

            if (result is null)
                return NotFound($"Doctor with id {doctorId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
            new { message = "An error occurred while deleting doctor." });

            _loggerService.Log($"Deleted doctor with id: {doctorId}.", ip, userID);
            return Ok(result);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [Authorize(Policy = "AddEditDoctors")]
        [HttpGet(Name = "GetAllDoctors")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<List<DoctorDto>>> GetAllDoctors()
        {
            List<DoctorDto> doctors = await _doctorService.GetAllDoctorsAsync();

            if (doctors is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
            new { message = "An error occurred while getting all doctors." });

            return Ok(doctors);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("todays-appointments/{doctorId}", Name = "GetTodaysAppointmentsForDoctor")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<IEnumerable<AppointmentForDoctorDto>>> GetTodaysAppointmentsForDoctor(int doctorId)
        {
            if (!DoctorValidation.ValidateDoctorId(doctorId))
                return BadRequest("Please validate your input.");

            int? userId = await _doctorService.FindUserIdForDoctor(doctorId);

            if (userId is null)
                return NotFound($"Doctor with id: {doctorId} is not found.");

            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(
                User,
                userId,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); 

            List<AppointmentForDoctorDto> appointments = await _doctorService.GetTodaysAppointmentsForDoctorAsync(doctorId);

            if (appointments is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
            new { message = "An error occurred while getting today's appointments for doctor." });

            return Ok(appointments);
        }

        [Authorize]
        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("patients/{doctorId}", Name = "PatientsCountForDoctor")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<int>> PatientsCountForDoctor(int doctorId)
        {
            if (!DoctorValidation.ValidateDoctorId(doctorId))
                return BadRequest("Please validate your input.");

            int? count = await _doctorService.PatientsCountForDoctorAsync(doctorId);

            if (count is null)
                return NotFound($"Doctor with id: {doctorId} is not found.");

            return Ok(count);
        }

        [Authorize]
        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("appointments/{doctorId}", Name = "AppointmentsCountForDoctor")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<int>> AppointmentsCountForDoctor(int doctorId)
        {
            if (!DoctorValidation.ValidateDoctorId(doctorId))
                return BadRequest("Please validate your input.");

            int? count = await _doctorService.AppointmentsCountForDoctorAsync(doctorId);

            if (count is null)
                return NotFound($"Doctor with id: {doctorId} is not found.");

            return Ok(count);
        }

        [Authorize]
        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("medicalrecords/{doctorId}", Name = "MedicalRecordsCountForDoctor")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<int>> MedicalRecordsCountForDoctor(int doctorId)
        {
            if (!DoctorValidation.ValidateDoctorId(doctorId))
                return BadRequest("Please validate your input.");

            int? count = await _doctorService.MedicalRecordsCountForDoctorAsync(doctorId);

            if (count is null)
                return NotFound($"Doctor with id: {doctorId} is not found.");

            return Ok(count);
        }
    }
}
