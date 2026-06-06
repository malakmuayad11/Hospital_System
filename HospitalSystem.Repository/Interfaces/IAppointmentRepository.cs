using HospitalSystem.API.Models;
using HospitalSystem.DTOs.Appointments;

namespace HospitalSystem.Repository.Interfaces
{
    public interface IAppointmentRepository
    {
        public Task<int> CountAsync();

        public Task<List<AppointmentDto>> GetAllAppointmentsAsync();

        public Task<List<AppointmentDto>> GetTodaysAppointmentsAsync();

        public Task<int?> AddNewAppointmentAsync(Appointment appointment);

        public Task<bool?> UpdateAppointmentDateTimeAsync(int  appointmentId, DateOnly appointmentDate, TimeOnly appointmentTime);
    
        public Task<bool?> UpdateAppointmentStatusAsync(int appointmentId, byte newStatus);
    
        public Task<Appointment> FindAsync(int appointmentId);

        public Task<bool?> CancelAppointmentAsync(int appointmentId);

        public Task<bool?> RescheduleAppointmentAsync(int appointmentId, DateOnly newDate, TimeOnly newTime);

        public Task<bool?> HasAppointmentMedicalRecordAsync(int appointmentId);
    }
}
