using System;
using System.Collections.Generic;

namespace HospitalSystem.API.Models;

public partial class Prescription
{
    public int PrescriptionId { get; set; }

    public int AppointmentId { get; set; }

    public string MedicationName { get; set; } = null!;

    public string Dosage { get; set; } = null!;

    public byte DurationDays { get; set; }

    public byte DurationMonths { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual ICollection<PrescriptionAuditTrail> PrescriptionAuditTrails { get; set; } = new List<PrescriptionAuditTrail>();
}
