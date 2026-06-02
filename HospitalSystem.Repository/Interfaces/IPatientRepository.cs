using HospitalSystem.API.Models;
using HospitalSystem.DTOs.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Repository.Interfaces
{
    public interface IPatientRepository
    {
        public Task<List<PatientDto>> GetAllPatientsAsync();

        public Task<int?> AddNewPatientAsync(Patient patient);

        public Task<bool?> UpdatePatientAsync(Patient updatedPatient);

        public Task<Patient> FindAsync(int patientId);

        public Task<Patient> FindAsync(string nationalNo);

        public Task<bool?> DeletePatientAsync(int patientId);

        public Task<bool> DoesNationalNoExistAsync(string nationalNo);

        public Task<bool?> HasPatientMedicalRecordsAsync(int patientId);

        public Task<bool?> HasPatientPrescriptionsAsync(int patientId);

        public Task<bool?> HasPatientAppointmentAtAsync(int patientId, DateOnly appointmentDate, TimeOnly appointmentTime);
    }
}
