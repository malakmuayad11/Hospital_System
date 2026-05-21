namespace HospitalSystem.DTOs
{
    public class UpdatePatientDto 
    {
        public int PatientId { get; set; }
        public string NationalNo { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; }
        public string EmergencyContact { get; set; }
    }
}
