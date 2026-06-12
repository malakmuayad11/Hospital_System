using HospitalSystem.API.Models;
using HospitalSystem.Infrastructure.DTOs.Appointments;
using HospitalSystem.Infrastructure.DTOs.Doctors;

namespace HospitalSystem.Repository.Interfaces
{
    public interface IDoctorRepository
    {
        public Task<int> DoctorsCount();

        public Task<int?> AddNewDoctorAsync(Doctor doctor);

        public Task<bool?> UpdateDoctorAsync(Doctor doctor);

        public Task<Doctor> FindAsync(int doctorId);

        public Task<Doctor> FindByUserIdAsync(int userId);

        public Task<bool?> DeleteDoctorAsync(int doctorId);

        public Task<List<DoctorDto>> GetAllDoctorsAsync();

        public Task<List<AppointmentForDoctorDto>> GetTodaysAppointmentsForDoctorAsync(int doctorId);

        public Task<int?> PatientsCountForDoctorAsync(int doctorId);

        public Task<int?> AppointmentsCountForDoctorAsync(int doctorId);

        public Task<int?> MedicalRecordsCountForDoctorAsync(int doctorId);

        public Task<int?> FindUserIdForDoctor(int doctorId);
    }
}
