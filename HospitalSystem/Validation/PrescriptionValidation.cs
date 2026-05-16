using HospitalSystem.DTOs;

namespace HospitalSystem.API.Validation
{
    public class PrescriptionValidation
    {
        public static bool ValidateAddPrescriptionDto(AddPrescriptionDto addPrescriptionDto)
            => addPrescriptionDto.AppointmentId > 0
            && !string.IsNullOrEmpty(addPrescriptionDto.MedicationName)
            && !string.IsNullOrEmpty(addPrescriptionDto.Dosage)
            && addPrescriptionDto.DurationDays > 0;
    }
}
