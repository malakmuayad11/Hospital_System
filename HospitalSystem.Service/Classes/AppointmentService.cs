using HospitalSystem.Data.Entities;
using HospitalSystem.Infrastructure.DTOs.Appointments;
using HospitalSystem.Repository.Interfaces;
using HospitalSystem.Service.Interfaces;

namespace HospitalSystem.Service.Classes
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository) => _appointmentRepository = appointmentRepository;

        public async Task<int> CountAsync() => await _appointmentRepository.CountAsync();

        public async Task<List<AppointmentDto>> GetTodaysAppointmentsAsync() =>
            await _appointmentRepository.GetTodaysAppointmentsAsync();

        public async Task<List<AppointmentDto>> GetAllAppointmentsAsync() =>
            await _appointmentRepository.GetAllAppointmentsAsync();

        public async Task<int?> AddNewAppointmentAsync(AddAppointmentDto addAppointmentDto)
        {
            Appointment appointment = new Appointment
            {
                DoctorId = addAppointmentDto.DoctorId,
                PatientId = addAppointmentDto.PatientId,
                AppointmentDate = addAppointmentDto.AppointmentDate,
                AppointmentTime = addAppointmentDto.AppointmentTime,
                ReasonForVisit = addAppointmentDto.ReasonForVisit
            };

            return await _appointmentRepository.AddNewAppointmentAsync(appointment);
        }

        public async Task<bool?> UpdateAppointmentDateTimeAsync(UpdateAppointmentDateTimeDto updateAppointmentDateTimeDto)
            => await _appointmentRepository.UpdateAppointmentDateTimeAsync(updateAppointmentDateTimeDto.AppointmentId,
                updateAppointmentDateTimeDto.AppointmentDate, updateAppointmentDateTimeDto.AppointmentTime);

        public async Task<bool?> UpdateAppointmentStatusAsync(UpdateAppointmentStatusDto updateAppointmentStatusDto) =>
            await _appointmentRepository.UpdateAppointmentStatusAsync(updateAppointmentStatusDto.AppointmentId,
                updateAppointmentStatusDto.Status);

        public async Task<FindAppointmentDto> FindAsync(int appointmentId)
        {
            Appointment appointment = await _appointmentRepository
                .FindAsync(appointmentId);


            if (appointment is null)
                return null;

            return new FindAppointmentDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.AppointmentDate,
                AppointmentTime = appointment.AppointmentTime,
                ReasonForVisit = appointment.ReasonForVisit,
                Status = appointment.Status
            };
        }

        public async Task<bool?> CancelAppointmentAsync(int appointmentId) =>
            await _appointmentRepository.CancelAppointmentAsync(appointmentId);

        public async Task<bool?> RescheduleAppointmentAsync(RescheduleAppointmentDto rescheduleAppointmentDto) => await _appointmentRepository.RescheduleAppointmentAsync(rescheduleAppointmentDto.AppointmentId,
                rescheduleAppointmentDto.NewAppointmentDate, rescheduleAppointmentDto.NewAppointmentTime);

        public async Task<bool?> HasAppointmentMedicalRecordAsync(int appointmentId) =>
            await _appointmentRepository.HasAppointmentMedicalRecordAsync(appointmentId);
    }
}
