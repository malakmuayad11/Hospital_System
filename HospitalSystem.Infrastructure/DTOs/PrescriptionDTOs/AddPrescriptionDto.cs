namespace HospitalSystem.Infrastructure.DTOs.Prescriptions
{
    public class AddPrescriptionDto
    {
        public int AppointmentId { get; set; }
        public string MedicationName { get; set; }
        public string Dosage {  get; set; }

        public byte DurationDays { get; set; }

        public byte DurationMonths { get; set; }
    }
}
