using HospitalSystem.Data.Entities;
using HospitalSystem.Infrastructure.DTOs.Appointments;
using HospitalSystem.Infrastructure.DTOs.Doctors;
using HospitalSystem.Repository.Interfaces;
using HospitalSystem.Service.Interfaces;

namespace HospitalSystem.Service.Classes
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasher;

        public DoctorService(IDoctorRepository doctorRepository, IUserRepository userRepository, IPasswordHasherService passwordHasher)
        {
            _doctorRepository = doctorRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<int> DoctorsCount() => await _doctorRepository.DoctorsCount();

        public async Task<int?> AddNewDoctorAsync(AddDoctorDto addDoctorDto)
        {
            // we need first to add a user for the doctor, then we can add the doctor with the user id

            // Generate a unique username for the doctor, for example "Doctor1", "Doctor2", etc.
            int doctorUsernameId = await DoctorsCount() + 1;

            int? userId = await _userRepository.AddUserAsync(new Data.Entities.User
            {
                Username = $"Doctor{doctorUsernameId}",
                Password = _passwordHasher.ComputeHash(addDoctorDto.Password), // this should be handled
                Role = 3, // Doctor role
                Permissions = 18 // Permissions for doctor role
            });

            // If user creation failed, return false
            if (userId == null)
                return null;

            // Now we can create the doctor with the generated userId
            Doctor doctor = new Doctor
            {
                PersonId = addDoctorDto.PersonId,
                StartWorkDay = addDoctorDto.StartWorkDay,
                EndWorkDay = addDoctorDto.EndWorkDay,
                StartWorkHour = addDoctorDto.StartWorkHour,
                EndWorkHour = addDoctorDto.EndWorkHour,
                ConsultationId = addDoctorDto.ConsultationId,
                UserId = userId.Value
            };

            return await _doctorRepository.AddNewDoctorAsync(doctor);
        }

        public async Task<bool?> UpdateDoctorAsync(UpdateDoctorDto updateDoctorDto)
        {
            Doctor doctor = new Doctor
            {
                DoctorId = updateDoctorDto.DoctorId,
                UserId = updateDoctorDto.UserId,
                StartWorkDay = updateDoctorDto.StartWorkDay,
                EndWorkDay = updateDoctorDto.EndWorkDay,
                StartWorkHour = updateDoctorDto.StartWorkHour,
                EndWorkHour = updateDoctorDto.EndWorkHour,
                ConsultationId = updateDoctorDto.ConsultationId
            };
            return await _doctorRepository.UpdateDoctorAsync(doctor);
        }

        public async Task<FindDoctorDto> FindAsync(int doctorId)
        {
            Doctor doctor = await _doctorRepository.FindAsync(doctorId);

            if (doctor == null)
                return null;

            return new FindDoctorDto
            {
                DoctorId = doctor.DoctorId,
                PersonId = doctor.PersonId,
                StartWorkDay = doctor.StartWorkDay,
                EndWorkDay = doctor.EndWorkDay,
                StartWorkHour = doctor.StartWorkHour,
                EndWorkHour = doctor.EndWorkHour,
                ConsultationId = doctor.ConsultationId,
                UserId = doctor.UserId ?? 0
            };
        }

        public async Task<FindDoctorDto> FindByUserIdAsync(int userId)
        {
            Doctor doctor = await _doctorRepository.FindByUserIdAsync(userId);

            if (doctor == null)
                return null;

            return new FindDoctorDto
            {
                DoctorId = doctor.DoctorId,
                PersonId = doctor.PersonId,
                StartWorkDay = doctor.StartWorkDay,
                EndWorkDay = doctor.EndWorkDay,
                StartWorkHour = doctor.StartWorkHour,
                EndWorkHour = doctor.EndWorkHour,
                ConsultationId = doctor.ConsultationId,
                UserId = doctor.UserId ?? 0
            };
        }

        public async Task<bool?> DeleteDoctorAsync(int doctorId) =>
             await _doctorRepository.DeleteDoctorAsync(doctorId);

        public async Task<List<DoctorDto>> GetAllDoctorsAsync() =>
            await _doctorRepository.GetAllDoctorsAsync();

        public async Task<List<AppointmentForDoctorDto>> GetTodaysAppointmentsForDoctorAsync(int doctorId) =>
            await _doctorRepository.GetTodaysAppointmentsForDoctorAsync(doctorId);

        public async Task<int?> PatientsCountForDoctorAsync(int doctorId) =>
            await _doctorRepository.PatientsCountForDoctorAsync(doctorId);

        public async Task<int?> AppointmentsCountForDoctorAsync(int doctorId) =>
            await _doctorRepository.AppointmentsCountForDoctorAsync(doctorId);

        public async Task<int?> MedicalRecordsCountForDoctorAsync(int doctorId) =>
            await _doctorRepository.MedicalRecordsCountForDoctorAsync(doctorId);

        public async Task<int?> FindUserIdForDoctor(int doctorId) =>
            await _doctorRepository.FindUserIdForDoctor(doctorId);
    }
}
