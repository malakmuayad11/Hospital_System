using HospitalSystem.Infrastructure.DTOs.Billings;
using HospitalSystem.Service.Interfaces;
using HospitalSystem.Service.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace HospitalSystem.API.Controllers
{
    [Authorize(Policy = "ManagePayments")]
    [Route("api/billings")]
    [ApiController]
    public class BillingsController : ControllerBase
    {
        private readonly IBillingService _billingService;
        private readonly ILoggerService _loggerService;

        public BillingsController(IBillingService billingService, ILoggerService loggerService)
        {
            _billingService = billingService;
            _loggerService = loggerService;
        }

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
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";

            if (!BillingValidation.ValidateUpdateBillingChargesDto(updateBillingChargesDto))
                return BadRequest("Please validate your input");

            bool? result = await _billingService.AddAdditionalCharges(updateBillingChargesDto);

            if (result is null)
                return NotFound($"Billing with id: {updateBillingChargesDto.BillingId} is not found.");

            if(result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while updating the billing's charges." });

            _loggerService.Log($"Updated billing's charges with billingID: {updateBillingChargesDto.BillingId}.",
                ip, userID);
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
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";

            if (!BillingValidation.ValidateUpdateBillingPaymentStatusDto(updateBillingPaymentStatusDto))
                return BadRequest("Please validate your input");

            bool? result = await _billingService.UpdateBillingPaymentStatus(updateBillingPaymentStatusDto);

            if (result is null)
                return NotFound($"Billing with id: {updateBillingPaymentStatusDto.BillingId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while updating the billing's payment status." });

            _loggerService.Log($"Updated billing's status with billingID: {updateBillingPaymentStatusDto.BillingId}.",
                ip, userID);
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
