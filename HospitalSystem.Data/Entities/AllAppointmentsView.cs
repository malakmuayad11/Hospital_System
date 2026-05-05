namespace HospitalSystem.API.Models;

public partial class AllAppointmentsView
{
    public int AppointmentId { get; set; }

    public string? PatientName { get; set; }

    public string NationalNo { get; set; } = null!;

    public string? DoctorName { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public string? Status { get; set; }
}
