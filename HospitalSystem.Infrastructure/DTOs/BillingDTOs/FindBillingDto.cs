namespace HospitalSystem.Infrastructure.DTOs.Billings
{
    public class FindBillingDto
    {
        public int BillingID { get; set; }
        public int AppointmentId { get; set; }
        public decimal ConsultationFee { get; set; }
        public decimal? AdditionalCharges { get; set; }
        public bool IsPaid { get; set; }
        public byte? PaymentMethod { get; set; }
    }
}
