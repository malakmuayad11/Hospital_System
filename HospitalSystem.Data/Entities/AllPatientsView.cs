using System;
using System.Collections.Generic;

namespace HospitalSystem.API.Models;

public partial class AllPatientsView
{
    public int PatientId { get; set; }

    public string? FullName { get; set; }

    public string NationalNo { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string Gender { get; set; } = null!;

    public string EmergencyContact { get; set; } = null!;
}
