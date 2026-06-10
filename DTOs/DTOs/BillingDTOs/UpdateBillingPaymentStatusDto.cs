namespace HospitalSystem.Infrastructure.DTOs.Billings
{
    public class UpdateBillingPaymentStatusDto
    {
        public int BillingId { get; set; }
        public bool IsPaid { get; set; }
        public byte? PaymentMethod { get; set; }
    }
}
