using HospitalSystem.API.Validation;
using HospitalSystem.DTOs.Appointments;
using HospitalSystem.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService) => _appointmentService = appointmentService;

        [HttpGet("count", Name = "AppointmentsCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> GetAppointmentsCountAsync() =>
            Ok(await _appointmentService.CountAsync());

        [HttpGet(Name = "GetAllAppointments")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAllAppointmentsAsync()
        {
            List<AppointmentDto> appointmentDtos = await _appointmentService.GetAllAppointmentsAsync();

            if (appointmentDtos is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while getting all appointments." });

            return Ok(appointmentDtos);
        }

        [HttpGet("todays-appointments", Name = "GetTodaysAppointments")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetTodaysAppointmentsAsync()
        {
            List<AppointmentDto> todaysAppointments = await _appointmentService.GetTodaysAppointmentsAsync();

            if (todaysAppointments is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while getting today's appointments." });

            return Ok(todaysAppointments);
        }


        [HttpPost(Name = "AddNewAppointment")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AddAppointmentDto>> AddNewAppointmentAsync(AddAppointmentDto addAppointmentDto)
        {
            if (!AppointmentValidation.ValidateAddAppointmentDto(addAppointmentDto))
                return BadRequest("Please validate your input.");

            int? appointmentId = await _appointmentService.AddNewAppointmentAsync(addAppointmentDto);

            if (appointmentId is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while adding the new appointment." });

            return CreatedAtRoute("FindAppointmentById", new { appointmentId = appointmentId }, addAppointmentDto);
        }

        [HttpPatch(Name = "UpdateAppointmentDateTime")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateAppointmentDateTimeAsync(UpdateAppointmentDateTimeDto updateAppointmentDateTimeDto)
        {
            if (!AppointmentValidation.ValidateUpdateAppointmentDateTimeDto(updateAppointmentDateTimeDto))
                return BadRequest("Please validate your input");

            bool? result = await _appointmentService.UpdateAppointmentDateTimeAsync(updateAppointmentDateTimeDto);

            if (result is null)
                return NotFound($"Appointment with id: {updateAppointmentDateTimeDto.AppointmentId} is not found");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while updating the user's last login date." });

            return Ok(result);
        }

        [HttpPatch("status", Name = "UpdateAppointmentStatus")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateAppointmentStatusAsync(UpdateAppointmentStatusDto updateAppointmentStatusDto)
        {
            if (!AppointmentValidation.ValidateUpdateAppointmentStatusDto(updateAppointmentStatusDto))
                return BadRequest("Please validate your input");

            bool? result = await _appointmentService.UpdateAppointmentStatusAsync(updateAppointmentStatusDto);

            if (result is null)
                return NotFound($"Appointment with id: {updateAppointmentStatusDto.AppointmentId} is not found");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while updating the appointment status." });

            return Ok(result);
        }

        [HttpGet("{appointmentId}", Name = "FindAppointmentById")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<FindAppointmentDto>> FindAsync(int appointmentId)
        {
            if (!AppointmentValidation.ValidateAppointmentId(appointmentId))
                return BadRequest("Please validate your input");

            FindAppointmentDto appointmentDto = await _appointmentService.FindAsync(appointmentId);
            
            if (appointmentDto is null)
                return NotFound($"Appointment with id: {appointmentId} is not found");

            return Ok(appointmentDto);
        }

        [HttpPatch("cancel/{appointmentId}", Name = "CancelAppointment")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CancelAppointmentAsync(int appointmentId)
        {
            if (!AppointmentValidation.ValidateAppointmentId(appointmentId))
                return BadRequest("Please validate your input");

            bool? result = await _appointmentService.CancelAppointmentAsync(appointmentId);

            if (result is null)
                return NotFound($"Appointment with id: {appointmentId} is not found");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while canceling the appointment." });

            return Ok(result);
        }

        [HttpPatch("reschedule", Name = "RescheduleAppointment")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> RescheduleAppointmentAsync(RescheduleAppointmentDto rescheduleAppointmentDto)
        {
            if (!AppointmentValidation.ValidateRescheduleAppointmentDto(rescheduleAppointmentDto))
                return BadRequest("Please validate your input");

            bool? result = await _appointmentService.RescheduleAppointmentAsync(rescheduleAppointmentDto);
            
            if (result is null)
                return NotFound($"Appointment with id: {rescheduleAppointmentDto.AppointmentId} is not found");
            
            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while rescheduling the appointment." });

            return Ok(result);
        }

        [HttpGet("{appointmentId}/has-medical-record", Name = "HasAppointmentMedicalRecord")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> HasAppointmentMedicalRecordAsync(int appointmentId)
        {
            if (!AppointmentValidation.ValidateAppointmentId(appointmentId))
                return BadRequest("Please validate your input");

            bool? result = await _appointmentService.HasAppointmentMedicalRecordAsync(appointmentId);
            
            if (result is null)
                return NotFound($"Appointment with id: {appointmentId} is not found");

            return Ok(result);
        }
    }
}
