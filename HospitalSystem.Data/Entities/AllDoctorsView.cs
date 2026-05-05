using System;
using System.Collections.Generic;

namespace HospitalSystem.API.Models;

public partial class AllDoctorsView
{
    public int DoctorId { get; set; }

    public string? Name { get; set; }

    public string? StartWorkDay { get; set; }

    public string? EndWorkDay { get; set; }

    public TimeOnly StartWorkHour { get; set; }

    public TimeOnly EndWorkHour { get; set; }

    public string Specialty { get; set; } = null!;
}
