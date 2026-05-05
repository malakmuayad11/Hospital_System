using System;
using System.Collections.Generic;

namespace HospitalSystem.API.Models;

public partial class MedicalRecordAuditTrail
{
    public int TreatmentAuditId { get; set; }

    public int MedicalRecordId { get; set; }

    public int AuditId { get; set; }

    public virtual AuditTrail Audit { get; set; } = null!;

    public virtual MedicalRecord MedicalRecord { get; set; } = null!;
}
