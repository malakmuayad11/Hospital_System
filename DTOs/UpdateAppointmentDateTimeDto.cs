namespace HospitalSystem.DTOs
{
    public class UpdateAppointmentDateTimeDto
    {
        public int AppointmentId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly AppointmentTime { get; set; }
    }
}
