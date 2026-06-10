using HospitalSystem.API.Models;
using HospitalSystem.Infrastructure.DTOs.Patients;
using HospitalSystem.Repository.Interfaces;
using HospitalSystem.Service.Interfaces;

namespace HospitalSystem.Service.Classes
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository) => _patientRepository = patientRepository;

        public async Task<List<PatientDto>?> GetAllPatientsAsync() =>
            await _patientRepository.GetAllPatientsAsync() ?? null;

        public async Task<int?> AddNewPatientAsync(AddPatientDto addPatientDto)
        {
            Patient patient = new Patient
            {
                PersonId = addPatientDto.PersonId,
                NationalNo = addPatientDto.NationalNo,
                DateOfBirth = addPatientDto.DateOfBirth,
                Address = addPatientDto.Address,
                EmergencyContact = addPatientDto.EmergencyContact
            };
            return await _patientRepository.AddNewPatientAsync(patient);
        }

        public async Task<bool?> UpdatePatientAsync(UpdatePatientDto updatePatientDto)
        {
            Patient patient = new Patient
            {
                PatientId = updatePatientDto.PatientId,
                NationalNo = updatePatientDto.NationalNo,
                DateOfBirth = updatePatientDto.DateOfBirth,
                Address = updatePatientDto.Address,
                EmergencyContact = updatePatientDto.EmergencyContact
            };

            return await _patientRepository.UpdatePatientAsync(patient);
        }

        private PatientDto _MapPatient(Patient patient) =>
            new PatientDto
            {
                PatientId = patient.PatientId,
                FullName = patient.Person.FirstName + " " + patient.Person.LastName,
                NationalNo = patient.NationalNo,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Person.Gender == 1 ? "Male" : "Female",
                EmergencyContact = patient.EmergencyContact
            };

        public async Task<PatientDto> FindAsync(int patientId)
        {
            Patient patient = await _patientRepository.FindAsync(patientId);

            if (patient is null)
                return null;

            return _MapPatient(patient);
        }

        public async Task<PatientDto> FindAsync(string nationalNo)
        {
            Patient patient = await _patientRepository.FindAsync(nationalNo);

            if (patient is null)
                return null;

            return _MapPatient(patient);
        }

        public async Task<bool?> DeletePatientAsync(int patientId) =>
            await _patientRepository.DeletePatientAsync(patientId);

        public async Task<bool> DoesNationalNoExistAsync(string nationalNo) =>
            await _patientRepository.DoesNationalNoExistAsync(nationalNo);

        public async Task<bool?> HasPatientMedicalRecordsAsync(int patientId) =>
            await _patientRepository.HasPatientMedicalRecordsAsync(patientId);

        public async Task<bool?> HasPatientPrescriptionsAsync(int patientId) =>
            await _patientRepository.HasPatientPrescriptionsAsync(patientId);

        public async Task<bool?> HasPatientAppointmentAtAsync(int patientId,
            DateOnly appointmentDate, TimeOnly appointmentTime) =>
            await _patientRepository.HasPatientAppointmentAtAsync(patientId, appointmentDate, appointmentTime);
    }
}
