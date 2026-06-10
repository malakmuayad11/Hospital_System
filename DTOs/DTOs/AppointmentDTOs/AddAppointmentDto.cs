namespace HospitalSystem.Infrastructure.DTOs.Appointments
{
    public class AddAppointmentDto
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly AppointmentTime { get; set; }
        public string ReasonForVisit { get; set; }
    }
}
