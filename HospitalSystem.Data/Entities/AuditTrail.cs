using System;
using System.Collections.Generic;

namespace HospitalSystem.API.Models;

public partial class AuditTrail
{
    public int AuditId { get; set; }

    public int UserId { get; set; }

    /// <summary>
    /// 1- Add, 2- Update, 3- Delete
    /// </summary>
    public byte ActionPerformed { get; set; }

    public DateTime? ActionDate { get; set; }

    public virtual ICollection<AppointmentAuditTrail> AppointmentAuditTrails { get; set; } = new List<AppointmentAuditTrail>();

    public virtual ICollection<MedicalRecordAuditTrail> MedicalRecordAuditTrails { get; set; } = new List<MedicalRecordAuditTrail>();

    public virtual ICollection<PrescriptionAuditTrail> PrescriptionAuditTrails { get; set; } = new List<PrescriptionAuditTrail>();

    public virtual User User { get; set; } = null!;
}
