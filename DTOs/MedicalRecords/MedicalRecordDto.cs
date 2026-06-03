namespace HospitalSystem.DTOs.MedicalRecords
{
    public class MedicalRecordDto
    {
        public int MedicalRecordId { get; set; }    
        public int AppointmentId { get; set; }
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
        public string? MedicalRecordNotes { get; set; }
    }
}
