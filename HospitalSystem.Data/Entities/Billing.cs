namespace HospitalSystem.Data.Entities;

public partial class Billing
{
    public int BillingId { get; set; }

    public int AppointmentId { get; set; }

    public decimal ConsultationFee { get; set; }

    public decimal? AdditionalCharges { get; set; }

    public bool IsPaid { get; set; }

    /// <summary>
    /// 0 -> Credit Card, 1 -> Cash
    /// </summary>
    public byte? PaymentMethod { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;
}
