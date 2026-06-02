using HospitalSystem.API.Validation;
using HospitalSystem.DTOs;
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

            if(!await _mediicalRecordService.AddNewMedicalRecordAsync(addMedicalRecordDto))
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while adding the new medical record." });

            return Ok();
            //return CreatedAtRoute("GetUserByID", new { userID = user.UserID }, user.userDTO);
        }
    }
}
