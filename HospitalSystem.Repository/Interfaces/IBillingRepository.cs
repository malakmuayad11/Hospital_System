using HospitalSystem.API.Models;
using HospitalSystem.Infrastructure.DTOs.Billings;

namespace HospitalSystem.Repository.Interfaces
{
    public interface IBillingRepository
    {
        public Task<bool?> AddAdditonalCharges(int billingId, decimal additionalCharges);

        public Task<bool?> UpdateBillingPaymentStatus(int billingId, bool isPaid, byte? paymentMethod);

        public Task<List<BillingDto>> GetAllBillingsAsync();

        public Task<Billing> FindAsync(int billingId);
    }
}
