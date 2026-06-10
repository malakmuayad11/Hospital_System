namespace HospitalSystem.Infrastructure.DTOs.Appointments
{
    public class UpdateAppointmentStatusDto
    {
        public int AppointmentId { get; set; }
        public byte Status { get; set; }
    }
}
