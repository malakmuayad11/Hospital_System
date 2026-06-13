using HospitalSystem.Data.Entities;
using HospitalSystem.Data.Data;
using HospitalSystem.Infrastructure.DTOs.Billings;
using HospitalSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Repository.Classes
{
    public class BillingRepository : IBillingRepository
    {
        private readonly HospitalSystemContext _context;

        public BillingRepository(HospitalSystemContext context) => this._context = context;

        public async Task<bool?> AddAdditonalCharges(int billingId, decimal additionalCharges)
        {
            Billing billing = await FindAsync(billingId);

            if (billing is null)
                return null;

            billing.AdditionalCharges = (billing.AdditionalCharges ?? 0) + additionalCharges;
            return await _context.SaveChangesAsync() > 0;
        }
    
        public async Task<bool?> UpdateBillingPaymentStatus(int billingId, bool isPaid, byte? paymentMethod)
        {
            Billing billing = await FindAsync(billingId);

            if (billing is null)
                return null;

            billing.IsPaid = isPaid;
            billing.PaymentMethod = paymentMethod;

            return await _context.SaveChangesAsync() > 0;
        }
            
        private static string _GetPaymentMethod(byte? paymentMethod)
        {
            if (paymentMethod is null)
                return "Not paid";

            switch (paymentMethod)
            {
                case 0:
                    return "Credit Card";
                case 1:
                    return "Cash";
                default:
                    return "Unknown";
            }
        }

        public async Task<List<BillingDto>> GetAllBillingsAsync() =>
            await _context.Billings
            .Select(b => new BillingDto
            {
                BillingId = b.BillingId,
                AppointmentId = b.AppointmentId,
                ConsultationFee = b.ConsultationFee,
                AdditionalCharges = b.AdditionalCharges ?? 0,
                TotalAmount = b.ConsultationFee + (b.AdditionalCharges ?? 0),
                PaymentMethod = _GetPaymentMethod(b.PaymentMethod)
            })
            .AsNoTracking()
            .ToListAsync();

        public async Task<Billing> FindAsync(int billingId) =>
            await _context.Billings.FindAsync(billingId);
    }
}
