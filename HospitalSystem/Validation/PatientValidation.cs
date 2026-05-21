using HospitalSystem.DTOs;

namespace HospitalSystem.API.Validation
{
    public class PatientValidation
    {

        public static bool ValidatePatientId(int patientId) => patientId > 0;

        public static bool ValidateNationalNo(string nationalNo) => !string.IsNullOrEmpty(nationalNo);
        
        public static bool ValidateAddPatientDto(AddPatientDto addPatientDto) =>
            addPatientDto.PersonId > 0 &&
            ValidateNationalNo(addPatientDto.NationalNo) &&
            addPatientDto.DateOfBirth != null &&
            !string.IsNullOrEmpty(addPatientDto.Address) &&
            !string.IsNullOrEmpty(addPatientDto.EmergencyContact);

        public static bool ValidateUpdatePatientDto(UpdatePatientDto updatePatientDto) =>
            ValidatePatientId(updatePatientDto.PatientId) &&
            ValidateNationalNo(updatePatientDto.NationalNo) &&
            updatePatientDto.DateOfBirth != null &&
            !string.IsNullOrEmpty(updatePatientDto.Address) &&
            !string.IsNullOrEmpty(updatePatientDto.EmergencyContact);
    }
}
