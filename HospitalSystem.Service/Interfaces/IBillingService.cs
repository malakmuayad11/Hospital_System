using HospitalSystem.DTOs.Billings;

namespace HospitalSystem.Service.Interfaces
{
    public interface IBillingService
    {
        public Task<bool?> AddAdditionalCharges(AddAdditionalChargesDto addAdditionalChargesDto);

        public Task<bool?> UpdateBillingPaymentStatus(UpdateBillingPaymentStatusDto updateBillingPaymentStatusDto);

        public Task<List<BillingDto>> GetAllBillingsAsync();

        public Task<FindBillingDto> FindAsync(int billingId);
    }
}
