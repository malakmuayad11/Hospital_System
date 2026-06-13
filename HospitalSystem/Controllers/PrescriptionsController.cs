using HospitalSystem.API.Validation;
using HospitalSystem.Infrastructure.DTOs.Prescriptions;
using HospitalSystem.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HospitalSystem.API.Controllers
{
    [Authorize(Policy = "AddEditMedicalRecords")] // Prescriptions should be accessed via medical records
    [Route("api/prescription")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionsController(IPrescriptionService prescriptionService) =>
            this._prescriptionService = prescriptionService;

        [EnableRateLimiting("CriticalOpsLimiter")]
        [HttpPost(Name = "AddNewPrescription")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<PrescriptionDto>> AddNewPrescriptionAsync(AddPrescriptionDto addPrescriptionDto)
        {
            if (!PrescriptionValidation.ValidateAddPrescriptionDto(addPrescriptionDto))
                return BadRequest("Please validate your input.");

            if(!await _prescriptionService.AddNewPrescriptionAsync(addPrescriptionDto))
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while adding the new prescription." });

            return CreatedAtRoute("FindPrescriptionByAppointmentId", new { AppointmentId = addPrescriptionDto.AppointmentId }, addPrescriptionDto);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("{prescriptionId}", Name = "FindPrescriptionById")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<PrescriptionDto>> GetPrescriptionByPrescriptionIdAsync(int prescriptionId)
        {
            if (!PrescriptionValidation.ValidatePrescriptionId(prescriptionId))
                return BadRequest("Please validate your input");

            PrescriptionDto prescriptionDto = await _prescriptionService.GetPrescriptionByPrescriptionIdAsync(prescriptionId);

            if (prescriptionDto is null)
                return NotFound($"Prescription with id: {prescriptionId} is not found");

            return Ok(prescriptionDto);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("findByAppointmentId/{appointmentId}", Name = "FindPrescriptionByAppointmentId")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<PrescriptionDto>> GetPrescriptionByAppointmentIdAsync(int appointmentId)
        {
            if (!PrescriptionValidation.ValidateAppointmentId(appointmentId))
                return BadRequest("Please validate your input");

            PrescriptionDto prescriptionDto = await _prescriptionService.GetPrescriptionByAppointmentIdAsync(appointmentId);

            if (prescriptionDto is null)
                return NotFound($"Prescription with appointment id: {appointmentId} is not found");

            return Ok(prescriptionDto);
        }
    }
}
