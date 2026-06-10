namespace HospitalSystem.Infrastructure.DTOs.Appointments
{
    public class RescheduleAppointmentDto
    {
        public int AppointmentId { get; set; }

        public DateOnly NewAppointmentDate { get; set; }

        public TimeOnly NewAppointmentTime { get; set; }
    }
}
