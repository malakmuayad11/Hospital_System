using HospitalSystem.API.Validation;
using HospitalSystem.DTOs;
using HospitalSystem.Service;
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
        public async Task<ActionResult<bool>> AddNewPrescriptionAsync(AddPrescriptionDto addPrescriptionDto)
        {
            if (!PrescriptionValidation.ValidateAddPrescriptionDto(addPrescriptionDto))
                return BadRequest("Please validate your input.");

            if(!await _prescriptionService.AddNewPrescriptionAsync(addPrescriptionDto))
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while adding the new user." });

            return Ok();
            // but the new route here
        }
    }
}
