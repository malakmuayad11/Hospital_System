namespace HospitalSystem.DTOs
{
    public class PrescriptionDto
    {
        public int PrescriptionId { get; set; }
        public int AppointmentId { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public byte DurationDays { get; set; }
        public byte DurationMonths { get; set; }
    }
}
