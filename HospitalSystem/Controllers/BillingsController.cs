using HospitalSystem.Infrastructure.DTOs.Billings;
using HospitalSystem.Service.Interfaces;
using HospitalSystem.Service.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HospitalSystem.API.Controllers
{
    [Authorize(Policy = "ManagePayments")]
    [Route("api/billings")]
    [ApiController]
    public class BillingsController : ControllerBase
    {
        private readonly IBillingService _billingService;

        public BillingsController(IBillingService billingService) => _billingService = billingService;

        [EnableRateLimiting("CriticalOpsLimiter")]
        [HttpPatch(Name = "AddAdditionalCharges")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult> AddAdditionalCharges(AddAdditionalChargesDto updateBillingChargesDto)
        {
            if (!BillingValidation.ValidateUpdateBillingChargesDto(updateBillingChargesDto))
                return BadRequest("Please validate your input");

            bool? result = await _billingService.AddAdditionalCharges(updateBillingChargesDto);

            if (result is null)
                return NotFound($"Billing with id: {updateBillingChargesDto.BillingId} is not found.");

            if(result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while updating the billing's charges." });

            return Ok("Billing's charges are updated successfully.");
        }

        [EnableRateLimiting("CriticalOpsLimiter")]
        [HttpPatch("payment-status", Name = "UpdateBillingPaymentStatus")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult> UpdateBillingPaymentStatus(UpdateBillingPaymentStatusDto updateBillingPaymentStatusDto)
        {
            if (!BillingValidation.ValidateUpdateBillingPaymentStatusDto(updateBillingPaymentStatusDto))
                return BadRequest("Please validate your input");

            bool? result = await _billingService.UpdateBillingPaymentStatus(updateBillingPaymentStatusDto);

            if (result is null)
                return NotFound($"Billing with id: {updateBillingPaymentStatusDto.BillingId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while updating the billing's payment status." });

            return Ok("Billing's payment status is updated successfully.");
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet(Name = "GetAllBillings")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<IEnumerable<BillingDto>>> GetAllBillings()
        {
            List<BillingDto> billings = await _billingService.GetAllBillingsAsync();

            if(billings is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while getting all billings." });

            return Ok(billings);
        }

        [EnableRateLimiting("LightOpsLimiter")]
        [HttpGet("{billingId}", Name = "Find")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<FindBillingDto>> Find(int billingId)
        {
            if (!BillingValidation.ValidateBillingId(billingId))
                return BadRequest("Please validate your input.");

            FindBillingDto findBillingDto = await _billingService.FindAsync(billingId);

            if (findBillingDto is null)
                return NotFound($"Billing with id: {billingId} is not found.");

            return Ok(findBillingDto);
        }
    }
}
