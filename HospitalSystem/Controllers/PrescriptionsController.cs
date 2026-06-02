using HospitalSystem.API.Models;
using HospitalSystem.API.Validation;
using HospitalSystem.DTOs.Prescriptions;
using HospitalSystem.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    [Route("api/prescription")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionsController(IPrescriptionService prescriptionService) =>
            this._prescriptionService = prescriptionService;

        [HttpPost(Name = "AddNewPrescription")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<PrescriptionDto>> AddNewPrescriptionAsync(AddPrescriptionDto addPrescriptionDto)
        {
            if (!PrescriptionValidation.ValidateAddPrescriptionDto(addPrescriptionDto))
                return BadRequest("Please validate your input.");

            if(!await _prescriptionService.AddNewPrescriptionAsync(addPrescriptionDto))
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while adding the new prescription." });

            return Ok();
            //return CreatedAtRoute("FindPrescriptionByAppointmentId", new { AppointmentId = addPrescriptionDto.AppointmentId }, addPrescriptionDto);
        }

        [HttpGet("{PrescriptionId}", Name = "FindPrescriptionById")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PrescriptionDto>> GetPrescriptionByPrescriptionIdAsync(int PrescriptionId)
        {
            if (!PrescriptionValidation.ValidatePrescriptionId(PrescriptionId))
                return BadRequest("Please validate your input");

            PrescriptionDto prescriptionDto = await _prescriptionService.GetPrescriptionByPrescriptionIdAsync(PrescriptionId);

            if (prescriptionDto == null)
                return NotFound($"Prescription with id: {PrescriptionId} is not found");

            return Ok(prescriptionDto);
        }

        [HttpGet("findByAppointmentId/{AppointmentId}", Name = "FindPrescriptionByAppointmentId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PrescriptionDto>> GetPrescriptionByAppointmentIdAsync(int AppointmentId)
        {
            if (!PrescriptionValidation.ValidateAppointmentId(AppointmentId))
                return BadRequest("Please validate your input");

            PrescriptionDto prescriptionDto = await _prescriptionService.GetPrescriptionByAppointmentIdAsync(AppointmentId);

            if (prescriptionDto == null)
                return NotFound($"Prescription with appointment id: {AppointmentId} is not found");

            return Ok(prescriptionDto);
        }
    }
}
