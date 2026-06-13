namespace HospitalSystem.Data.Entities;

public partial class PatientsMedicalRecordsAndPrescription
{
    public int AppointmentId { get; set; }

    public int PatientId { get; set; }

    public int MedicalRecordId { get; set; }

    public int PrescriptionId { get; set; }
}
