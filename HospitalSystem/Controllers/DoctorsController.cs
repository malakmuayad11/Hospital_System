using HospitalSystem.DTOs.Appointments;
using HospitalSystem.DTOs.Doctors;
using HospitalSystem.Service.Interfaces;
using HospitalSystem.Service.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        public DoctorsController(IDoctorService doctorService) => _doctorService = doctorService;

        [HttpGet("count", Name = "GetDoctorsCount")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<int>> GetDoctorsCount()
        {
            int count = await _doctorService.DoctorsCount();
            if (count == null)
                return StatusCode(StatusCodes.Status500InternalServerError,
            new { message = "An error occurred while getting doctors count." });

            return Ok(count);
        }

        [HttpPost(Name = "AddNewDoctor")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> AddNewDoctor(AddDoctorDto addDoctorDto)
        {
            if (!DoctorValidation.ValidateAddDoctorDto(addDoctorDto))
                return BadRequest("Please validate your input.");

            if (!await _doctorService.AddNewDoctorAsync(addDoctorDto))
                return StatusCode(StatusCodes.Status500InternalServerError,
            new { message = "An error occurred while adding new doctor." });

            return Ok(); // replace with get route
        }

        [HttpPut(Name = "UpdateDoctor")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateDoctor(UpdateDoctorDto updateDoctorDto)
        {
            if (!DoctorValidation.ValidateUpdateDoctorDto(updateDoctorDto))
                return BadRequest("Please validate your input.");

            bool? result = await _doctorService.UpdateDoctorAsync(updateDoctorDto);

            if (result == null)
                return NotFound($"Doctor with id {updateDoctorDto.DoctorId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
            new { message = "An error occurred while updating doctor." });

            return Ok(result);
        }

        [HttpGet("{doctorId}", Name = "FindDoctor")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<FindDoctorDto>> Find(int doctorId)
        {
            if (!DoctorValidation.ValidateDoctorId(doctorId))
                return BadRequest("Please validate your input.");

            FindDoctorDto doctorDto = await _doctorService.FindAsync(doctorId);

            if (doctorDto == null)
                return NotFound($"Doctor with id {doctorId} is not found.");

            return Ok(doctorDto);
        }

        [HttpGet("user/{userId}", Name = "FindDoctorByUserId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<FindDoctorDto>> FindByUserId(int userId)
        {
            if (!DoctorValidation.ValidateUserId(userId))
                return BadRequest("Please validate your input.");

            FindDoctorDto doctorDto = await _doctorService.FindByUserIdAsync(userId);

            if (doctorDto == null)
                return NotFound($"Doctor with user id {userId} is not found.");

            return Ok(doctorDto);
        }

        [HttpDelete("{doctorId}", Name = "DeleteDoctor")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteDoctor(int doctorId)
        {
            if (!DoctorValidation.ValidateDoctorId(doctorId))
                return BadRequest("Please validate your input.");

            bool? result = await _doctorService.DeleteDoctorAsync(doctorId);

            if (result == null)
                return NotFound($"Doctor with id {doctorId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
            new { message = "An error occurred while deleting doctor." });

            return Ok(result);
        }

        [HttpGet(Name = "GetAllDoctors")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DoctorDto>>> GetAllDoctors()
        {
            List<DoctorDto> doctors = await _doctorService.GetAllDoctorsAsync();

            if (doctors == null)
                return StatusCode(StatusCodes.Status500InternalServerError,
            new { message = "An error occurred while getting all doctors." });

            return Ok(doctors);
        }

        [HttpGet("todays-appointments/{doctorId}", Name = "GetTodaysAppointmentsForDoctor")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AppointmentForDoctorDto>>> GetTodaysAppointmentsForDoctor(int doctorId)
        {
            if (!DoctorValidation.ValidateDoctorId(doctorId))
                return BadRequest("Please validate your input.");

            List<AppointmentForDoctorDto> appointments = await _doctorService.GetTodaysAppointmentsForDoctorAsync(doctorId);

            if (appointments == null)
                return StatusCode(StatusCodes.Status500InternalServerError,
            new { message = "An error occurred while getting today's appointments for doctor." });

            return Ok(appointments);
        }

        [HttpGet("patients/{doctorId}", Name = "PatientsCountForDoctor")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> PatientsCountForDoctor(int doctorId)
        {
            if (!DoctorValidation.ValidateDoctorId(doctorId))
                return BadRequest("Please validate your input.");

            int? count = await _doctorService.PatientsCountForDoctorAsync(doctorId);

            if (count == null)
                return NotFound($"Doctor with id: {doctorId} is not found.");

            return Ok(count);
        }

        [HttpGet("appointments/{doctorId}", Name = "AppointmentsCountForDoctor")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> AppointmentsCountForDoctor(int doctorId)
        {
            if (!DoctorValidation.ValidateDoctorId(doctorId))
                return BadRequest("Please validate your input.");

            int? count = await _doctorService.AppointmentsCountForDoctorAsync(doctorId);

            if (count == null)
                return NotFound($"Doctor with id: {doctorId} is not found.");

            return Ok(count);
        }

        [HttpGet("medicalrecords/{doctorId}", Name = "MedicalRecordsCountForDoctor")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> MedicalRecordsCountForDoctor(int doctorId)
        {
            if (!DoctorValidation.ValidateDoctorId(doctorId))
                return BadRequest("Please validate your input.");

            int? count = await _doctorService.MedicalRecordsCountForDoctorAsync(doctorId);

            if (count == null)
                return NotFound($"Doctor with id: {doctorId} is not found.");

            return Ok(count);
        }

    }
}
