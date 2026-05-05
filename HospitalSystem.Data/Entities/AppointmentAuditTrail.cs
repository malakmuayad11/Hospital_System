using System;
using System.Collections.Generic;

namespace HospitalSystem.API.Models;

public partial class AppointmentAuditTrail
{
    public int AppointmentAuditId { get; set; }

    public int AppointmentId { get; set; }

    public int AuditId { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual AuditTrail Audit { get; set; } = null!;
}
