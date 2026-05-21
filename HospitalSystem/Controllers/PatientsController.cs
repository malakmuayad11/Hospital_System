using HospitalSystem.API.Validation;
using HospitalSystem.DTOs;
using HospitalSystem.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        public PatientsController(IPatientService patientService) => this._patientService = patientService;
        
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetAllPatientsAsync()
        {
            List<PatientDto> patientDtos = await _patientService.GetAllPatientsAsync();

            if (patientDtos == null)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while fetching patients." });

            return Ok(patientDtos);
        }

        [HttpPost(Name = "AddNewPatient")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<PatientDto>> AddNewPatientAsync(AddPatientDto addPatientDto)
        {
            if (!PatientValidation.ValidateAddPatientDto(addPatientDto))
                return BadRequest("Please validate your input.");

            int? patientId = await _patientService.AddNewPatientAsync(addPatientDto);

            if(patientId == null)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while adding the new patient." });

            return CreatedAtRoute("GetPatientByPatientId", new { patientId = patientId }, addPatientDto);
        }

        [HttpPut(Name = "UpdatePatient")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdatePatientAsync(UpdatePatientDto updatePatientDto)
        {
            if (!PatientValidation.ValidateUpdatePatientDto(updatePatientDto))
                return BadRequest("Please validate your input");

            bool? result = await _patientService.UpdatePatientAsync(updatePatientDto);

            if(result == null)
                return NotFound($"Patient with id: {updatePatientDto.PatientId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while updating the patient." });

            return Ok("Patient is updated successfully.");
        }


        [HttpGet("by-id/{patientId}", Name = "GetPatientByPatientId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PatientDto>> FindAsync(int patientId)
        {
            if (!PatientValidation.ValidatePatientId(patientId))
                return BadRequest("Please validate your input.");

            PatientDto patientDto = await _patientService.FindAsync(patientId);

            if (patientDto is null)
                return NotFound($"Patient with patientId: {patientId} is not found.");

            return Ok(patientDto);
        }

        [HttpGet("by-national/{nationalNo}", Name = "GetPatientByNationalNo")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PatientDto>> FindAsync(string nationalNo)
        {
            if (!PatientValidation.ValidateNationalNo(nationalNo))
                return BadRequest("Please validate your input.");

            PatientDto patientDto = await _patientService.FindAsync(nationalNo);

            if(patientDto is null)
                return NotFound($"Patient with nationalNo: {nationalNo} is not found.");

            return Ok(patientDto);
        }

        [HttpDelete("{patientId}", Name = "DeletePatient")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(int patientId)
        {
            if (!PatientValidation.ValidatePatientId(patientId))
                return BadRequest("Please validate your input.");

            bool? result = await _patientService.DeletePatientAsync(patientId);

            if (result is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while deleting the patient." });

            if (result == false)
                    return NotFound($"Patient with id: {patientId} is not found.");
            
            return Ok(result);
        }

        [HttpGet("national-no/{nationalNo}/exists", Name = "DoesNationalNoExist")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DoesNationalNoExistAsync(string nationalNo)
        {
            if (!PatientValidation.ValidateNationalNo(nationalNo))
                return BadRequest("Please validate your input.");

            return Ok(await _patientService.DoesNationalNoExistAsync(nationalNo));
        }

        [HttpGet("medical-records/{patientId}/exists", Name = "HasPatientMedicalRecords")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> HasPatientMedicalRecordsAsync(int patientId)
        {
            if (!PatientValidation.ValidatePatientId(patientId))
                return BadRequest("Please validate your input.");

            bool? result = await _patientService.HasPatientMedicalRecordsAsync(patientId);

            if(result is null)
                return NotFound($"Patient with id: {patientId} is not found.");

            return Ok(result);
        }

        [HttpGet("prescriptions/{patientId}/exists", Name = "HasPatientPrescriptions")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> HasPatientPrescriptionsAsync(int patientId)
        {
            if (!PatientValidation.ValidatePatientId(patientId))
                return BadRequest("Please validate your input.");

            bool? result = await _patientService.HasPatientPrescriptionsAsync(patientId);

            if (result is null)
                return NotFound($"Patient with id: {patientId} is not found.");

            return Ok(result);
        }

        [HttpGet("appointments/{patientId}/{appointmentDate}/{appointmentTime}/exists", Name = "HasPatientAppointmentAt")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> HasPatientAppointmentAtAsync(int patientId, 
            DateOnly appointmentDate, TimeOnly appointmentTime)
        {
            if (!PatientValidation.ValidatePatientId(patientId) 
                || !AppointmentValidation.ValidateAppointmentDate(appointmentDate)
                || !AppointmentValidation.ValidateAppointmentTime(appointmentTime))
                return BadRequest("Please validate your input.");

            bool? result = await _patientService.HasPatientAppointmentAtAsync(patientId,
                appointmentDate, appointmentTime);

            if (result is null)
                return NotFound($"Patient with id: {patientId} is not found.");

            return Ok(result);
        }
    }
}
