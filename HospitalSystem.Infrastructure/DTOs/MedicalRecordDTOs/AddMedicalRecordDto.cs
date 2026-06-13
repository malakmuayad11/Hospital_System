namespace HospitalSystem.Infrastructure.DTOs.MedicalRecords
{
    public class AddMedicalRecordDto
    {
        public int AppointmentId { get; set; }
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
        public string MedicalRecordNotes { get; set; }
    }
}
