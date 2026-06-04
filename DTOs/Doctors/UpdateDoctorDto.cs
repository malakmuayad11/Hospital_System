namespace HospitalSystem.DTOs.Doctors
{
    public class UpdateDoctorDto
    {
        public int DoctorId { get; set; }
        public byte StartWorkDay { get; set; }
        public byte EndWorkDay { get; set; }
        public TimeOnly StartWorkHour { get; set; }
        public TimeOnly EndWorkHour { get; set; }
        public int ConsultationId { get; set; }
    }
}
