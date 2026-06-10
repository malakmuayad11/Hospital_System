using HospitalSystem.Infrastructure.DTOs.Prescriptions;

namespace HospitalSystem.API.Validation
{
    public class PrescriptionValidation
    {
        public static bool ValidatePrescriptionId(int PrescriptionId) => PrescriptionId > 0;

        public static bool ValidateAppointmentId(int AppointmentId) => AppointmentId > 0;
        
        public static bool ValidateAddPrescriptionDto(AddPrescriptionDto addPrescriptionDto)
            => addPrescriptionDto.AppointmentId > 0
            && !string.IsNullOrEmpty(addPrescriptionDto.MedicationName)
            && !string.IsNullOrEmpty(addPrescriptionDto.Dosage)
            && addPrescriptionDto.DurationDays > 0;
    }
}
