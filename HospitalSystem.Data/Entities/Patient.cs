using System;
using System.Collections.Generic;

namespace HospitalSystem.API.Models;

public partial class Patient
{
    public int PatientId { get; set; }

    public int PersonId { get; set; }

    public string NationalNo { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string Address { get; set; } = null!;

    public string EmergencyContact { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Person Person { get; set; } = null!;
}
