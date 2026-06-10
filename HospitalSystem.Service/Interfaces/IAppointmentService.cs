using HospitalSystem.Infrastructure.DTOs.Appointments;

namespace HospitalSystem.Service.Interfaces
{
    public interface IAppointmentService
    {
        public Task<int> CountAsync();

        public Task<List<AppointmentDto>> GetTodaysAppointmentsAsync();

        public Task<List<AppointmentDto>> GetAllAppointmentsAsync();

        public Task<int?> AddNewAppointmentAsync(AddAppointmentDto addAppointmentDto);

        public Task<bool?> UpdateAppointmentDateTimeAsync(UpdateAppointmentDateTimeDto updateAppointmentDateTimeDto);

        public Task<bool?> UpdateAppointmentStatusAsync(UpdateAppointmentStatusDto updateAppointmentStatusDto);

        public Task<FindAppointmentDto> FindAsync(int appointmentId);

        public Task<bool?> CancelAppointmentAsync(int appointmentId);

        public Task<bool?> RescheduleAppointmentAsync(RescheduleAppointmentDto rescheduleAppointmentDto);

        public Task<bool?> HasAppointmentMedicalRecordAsync(int appointmentId);
    }
}
