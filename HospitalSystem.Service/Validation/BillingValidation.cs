using HospitalSystem.DTOs.Billings;

namespace HospitalSystem.Service.Validation
{
    public class BillingValidation
    {

        public static bool ValidateBillingId(int billingId) => billingId >= 0;
        public static bool ValidateUpdateBillingChargesDto(AddAdditionalChargesDto updateBillingChargesDto) =>
            ValidateBillingId(updateBillingChargesDto.BillingId) &&
            updateBillingChargesDto.AdditionalCharges > 0;

        public static bool ValidateUpdateBillingPaymentStatusDto(UpdateBillingPaymentStatusDto updateBillingPaymentStatusDto)
        {
            if (!ValidateBillingId(updateBillingPaymentStatusDto.BillingId))
                return false;

            if (updateBillingPaymentStatusDto.PaymentMethod is not null &&
                updateBillingPaymentStatusDto.PaymentMethod > 1)
                return false;

            return true;
        }
    }
}
