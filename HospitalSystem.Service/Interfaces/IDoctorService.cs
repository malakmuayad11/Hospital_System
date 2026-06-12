using HospitalSystem.Infrastructure.DTOs.Appointments;
using HospitalSystem.Infrastructure.DTOs.Doctors;

namespace HospitalSystem.Service.Interfaces
{
    public interface IDoctorService
    {
        public Task<int> DoctorsCount();
    
        public Task<int?> AddNewDoctorAsync(AddDoctorDto addDoctorDto);

        public Task<bool?> UpdateDoctorAsync(UpdateDoctorDto updateDoctorDto);

        public Task<FindDoctorDto> FindAsync(int doctorId);

        public Task<FindDoctorDto> FindByUserIdAsync(int userId);

        public Task<bool?> DeleteDoctorAsync(int doctorId);

        public Task<List<DoctorDto>> GetAllDoctorsAsync();

        public Task<List<AppointmentForDoctorDto>> GetTodaysAppointmentsForDoctorAsync(int doctorId);

        public Task<int?> PatientsCountForDoctorAsync(int doctorId);

        public Task<int?> AppointmentsCountForDoctorAsync(int doctorId);

        public Task<int?> MedicalRecordsCountForDoctorAsync(int doctorId);

        public Task<int?> FindUserIdForDoctor(int doctorId);

    }
}
