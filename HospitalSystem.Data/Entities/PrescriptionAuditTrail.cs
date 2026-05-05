using System;
using System.Collections.Generic;

namespace HospitalSystem.API.Models;

public partial class PrescriptionAuditTrail
{
    public int PrescriptionAuditId { get; set; }

    public int PrescriptionId { get; set; }

    public int AuditId { get; set; }

    public virtual AuditTrail Audit { get; set; } = null!;

    public virtual Prescription Prescription { get; set; } = null!;
}
