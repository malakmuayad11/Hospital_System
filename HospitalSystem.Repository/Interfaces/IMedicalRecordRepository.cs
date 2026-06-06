using HospitalSystem.API.Models;
using HospitalSystem.DTOs.MedicalRecords;

namespace HospitalSystem.Repository.Interfaces
{
    public interface IMedicalRecordRepository
    {
        public Task<int?> AddNewMedicalRecordAsync(MedicalRecord medicalRecord);

        public Task<List<MedicalRecordDto>> GetAllMedicalRecordsAsync();
        
        public Task<MedicalRecord> FindAsync(int medicalRecordId);

        public Task<MedicalRecord> FindByAppointmentIdAsync(int appointmentId);
    }
}
