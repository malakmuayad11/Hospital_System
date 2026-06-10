using HospitalSystem.Infrastructure.DTOs.Patients;

namespace HospitalSystem.Service.Interfaces
{
    public interface IPatientService
    {
        public Task<List<PatientDto>?> GetAllPatientsAsync();

        public Task<int?> AddNewPatientAsync(AddPatientDto addPatientDto);

        public Task<bool?> UpdatePatientAsync(UpdatePatientDto updatePatientDto);

        public Task<PatientDto> FindAsync(int patientId);

        public Task<PatientDto> FindAsync(string nationalNo);

        public Task<bool?> DeletePatientAsync(int patientId);

        public Task<bool> DoesNationalNoExistAsync(string nationalNo);

        public Task<bool?> HasPatientMedicalRecordsAsync(int patientId);

        public Task<bool?> HasPatientPrescriptionsAsync(int patientId);

        public Task<bool?> HasPatientAppointmentAtAsync(int patientId, DateOnly appointmentDate, TimeOnly appointmentTime);
    }
}
