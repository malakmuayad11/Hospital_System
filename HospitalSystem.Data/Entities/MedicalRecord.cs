using System;
using System.Collections.Generic;

namespace HospitalSystem.API.Models;

public partial class MedicalRecord
{
    public int MedicalRecordId { get; set; }

    public int AppointmentId { get; set; }

    public string Symptoms { get; set; } = null!;

    public string Diagnosis { get; set; } = null!;

    public string MedicalRecordNotes { get; set; } = null!;

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual ICollection<MedicalRecordAuditTrail> MedicalRecordAuditTrails { get; set; } = new List<MedicalRecordAuditTrail>();
}
