using HospitalSystem.Infrastructure.DTOs.MedicalRecords;

namespace HospitalSystem.API.Validation
{
    public class MedicalRecordValidation
    {
        public static bool ValidateMedicalRecordId(int medicalRecordId) => medicalRecordId > 0;

        public static bool ValidateAppointmentId(int appointmentId) => appointmentId > 0;
        public static bool ValidateAddMedicalRecordDto(AddMedicalRecordDto addMedicalRecordDto) =>
            ValidateAppointmentId(addMedicalRecordDto.AppointmentId)
            && !string.IsNullOrEmpty(addMedicalRecordDto.Symptoms)
            && !string.IsNullOrEmpty(addMedicalRecordDto.Diagnosis);
    }
}
