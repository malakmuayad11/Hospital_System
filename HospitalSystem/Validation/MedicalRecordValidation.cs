using HospitalSystem.DTOs;

namespace HospitalSystem.API.Validation
{
    public class MedicalRecordValidation
    {
        public static bool ValidateAddMedicalRecordDto(AddMedicalRecordDto addMedicalRecordDto) =>
            addMedicalRecordDto.AppointmentId > 0
            && !string.IsNullOrEmpty(addMedicalRecordDto.Symptoms)
            && !string.IsNullOrEmpty(addMedicalRecordDto.Diagnosis)
            && !string.IsNullOrEmpty(addMedicalRecordDto.MedicalRecordNotes);
    }
}
