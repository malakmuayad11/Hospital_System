namespace HospitalSystem.Infrastructure.DTOs.Billings
{
    public class BillingDto
    {
        public int BillingId { get; set; }
        public int AppointmentId { get; set; }
        public decimal ConsultationFee { get; set; }
        public decimal AdditionalCharges { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
    }
}
