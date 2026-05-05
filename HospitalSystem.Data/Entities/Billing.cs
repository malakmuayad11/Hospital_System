using System;
using System.Collections.Generic;

namespace HospitalSystem.API.Models;

public partial class Billing
{
    public int BillingId { get; set; }

    public int AppointmentId { get; set; }

    public decimal ConsultationFee { get; set; }

    public decimal? AdditionalCharges { get; set; }

    public bool IsPaid { get; set; }

    public byte? PaymentMethod { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;
}
