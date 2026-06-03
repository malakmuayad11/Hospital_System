using HospitalSystem.API.Validation;
using HospitalSystem.DTOs.MedicalRecords;
using HospitalSystem.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    [Route("api/medicalrecords")]
    [ApiController]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly IMedicalRecordService _mediicalRecordService;

        public MedicalRecordsController(IMedicalRecordService mediicalRecordService)
            => _mediicalRecordService = mediicalRecordService;

        [HttpPost(Name = "AddNewMedicalRecord")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> AddNewMedicalRecordAsync(AddMedicalRecordDto addMedicalRecordDto)
        {
            if (!MedicalRecordValidation.ValidateAddMedicalRecordDto(addMedicalRecordDto))
                return BadRequest("Please validate your input");

            if (!await _mediicalRecordService.AddNewMedicalRecordAsync(addMedicalRecordDto))
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while adding the new medical record." });

            return Ok();
            //return CreatedAtRoute("GetUserByID", new { userID = user.UserID }, user.userDTO);
        }

        [HttpGet(Name = "GetAllMedicalRecords")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MedicalRecordDto>>> GetAllMedicalRecordsAsync()
        {
            List<MedicalRecordDto> medicalRecords = await _mediicalRecordService.GetAllMedicalRecordsAsync();

            if (medicalRecords is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while fetching medical records." });

            return Ok(medicalRecords);
        }

        [HttpGet("{medicalRecordId}", Name = "FindMedicalRecordByID")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<MedicalRecordDto>> FindAsync(int medicalRecordId)
        {
            if (!MedicalRecordValidation.ValidateMedicalRecordId(medicalRecordId))
                return BadRequest("Please validate your input");

            MedicalRecordDto medicalRecord = await _mediicalRecordService.FindAsync(medicalRecordId);

            if (medicalRecord is null)
                return NotFound(new { message = $"Medical record with ID {medicalRecordId} not found." });

            return Ok(medicalRecord);
        }

        [HttpGet("appointment/{appointmentId}", Name = "FindMedicalRecordByAppointmentID")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<MedicalRecordDto>> FindByAppointmentIdAsync(int appointmentId)
        {
            if (!MedicalRecordValidation.ValidateAppointmentId(appointmentId))
                return BadRequest("Please validate your input");

            MedicalRecordDto medicalRecord = await _mediicalRecordService.FindByAppointmentIdAsync(appointmentId);
            if (medicalRecord is null)
                return NotFound(new { message = $"Medical record with appointment ID {appointmentId} not found." });

            return Ok(medicalRecord);
        }
    }
}
