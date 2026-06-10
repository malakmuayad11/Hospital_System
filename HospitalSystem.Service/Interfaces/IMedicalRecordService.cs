using HospitalSystem.Infrastructure.DTOs.MedicalRecords;

namespace HospitalSystem.Service.Interfaces
{
    public interface IMedicalRecordService
    {
        public Task<int?> AddNewMedicalRecordAsync(AddMedicalRecordDto addMedicalRecordDto);

        public Task<List<MedicalRecordDto>> GetAllMedicalRecordsAsync();

        public Task<MedicalRecordDto> FindAsync(int medicalRecordId);

        public Task<MedicalRecordDto> FindByAppointmentIdAsync(int appointmentId);
    }
}
