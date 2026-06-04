namespace HospitalSystem.DTOs.Appointments
{
    public class AppointmentForDoctorDto
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly AppointmentTime { get; set; }
        public string ReasonForVisit { get; set; }
        public byte Status { get; set; }
    }
}
