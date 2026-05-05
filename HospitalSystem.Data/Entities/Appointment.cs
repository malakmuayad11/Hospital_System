using System;
using System.Collections.Generic;

namespace HospitalSystem.API.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int DoctorId { get; set; }

    public int PatientId { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public TimeOnly AppointmentTime { get; set; }

    public string ReasonForVisit { get; set; } = null!;

    /// <summary>
    /// 1- Scheduled. 2- 
    /// </summary>
    public byte Status { get; set; }

    public bool? IsReminderSent { get; set; }

    public virtual ICollection<AppointmentAuditTrail> AppointmentAuditTrails { get; set; } = new List<AppointmentAuditTrail>();

    public virtual ICollection<Billing> Billings { get; set; } = new List<Billing>();

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual Patient Patient { get; set; } = null!;

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
