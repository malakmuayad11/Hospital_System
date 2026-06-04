using HospitalSystem.API.Models;
using HospitalSystem.DTOs.Billings;
using HospitalSystem.Repository.Interfaces;
using HospitalSystem.Service.Interfaces;

namespace HospitalSystem.Service.Classes
{
    public class BillingService : IBillingService
    {
        private readonly IBillingRepository _billingRepository;

        public BillingService(IBillingRepository billingRepository) => _billingRepository = billingRepository;

        public async Task<bool?> AddAdditionalCharges(AddAdditionalChargesDto updateBillingChargesDto) =>
            await _billingRepository.AddAdditonalCharges(updateBillingChargesDto.BillingId, updateBillingChargesDto.AdditionalCharges);

        public async Task<bool?> UpdateBillingPaymentStatus(UpdateBillingPaymentStatusDto updateBillingPaymentStatusDto) =>
            await _billingRepository.UpdateBillingPaymentStatus(updateBillingPaymentStatusDto.BillingId,
                updateBillingPaymentStatusDto.IsPaid, updateBillingPaymentStatusDto.PaymentMethod);

        public async Task<List<BillingDto>> GetAllBillingsAsync() =>
            await _billingRepository.GetAllBillingsAsync();

        public async Task<FindBillingDto> FindAsync(int billingId)
        {
            Billing billing = await _billingRepository.FindAsync(billingId);

            if (billing is null)
                return null;

            return new FindBillingDto
            {
                BillingID = billing.BillingId,
                AppointmentId = billing.AppointmentId,
                ConsultationFee = billing.ConsultationFee,
                AdditionalCharges = billing.AdditionalCharges,
                IsPaid = billing.IsPaid,
                PaymentMethod = billing.PaymentMethod
            };
        }
    }
}
