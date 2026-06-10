namespace HospitalSystem.Infrastructure.DTOs.Patients
{
    public class PatientDto
    {
        public int PatientId { get; set; }
        public string FullName { get; set; }
        public string NationalNo { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string EmergencyContact { get; set; }
    }
}
