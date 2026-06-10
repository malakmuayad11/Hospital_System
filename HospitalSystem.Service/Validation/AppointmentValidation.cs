using HospitalSystem.Infrastructure.DTOs.Appointments;

namespace HospitalSystem.API.Validation
{
    public class AppointmentValidation
    {
        public static bool ValidateAppointmentId(int appointmentId) => appointmentId > 0;
        public static bool ValidateAppointmentDate(DateOnly appointmentDate) =>
            appointmentDate >= DateOnly.FromDateTime(DateTime.Today);

        public static bool ValidateAppointmentTime(TimeOnly appointmentTime) =>
            appointmentTime > TimeOnly.FromDateTime(DateTime.Now);

        public static bool ValidateAddAppointmentDto(AddAppointmentDto addAppointmentDto) =>
            addAppointmentDto.DoctorId > 0 &&
            addAppointmentDto.PatientId > 0 &&
            ValidateAppointmentDate(addAppointmentDto.AppointmentDate)
            && ValidateAppointmentTime(addAppointmentDto.AppointmentTime)
            && !string.IsNullOrEmpty(addAppointmentDto.ReasonForVisit);

        public static bool ValidateUpdateAppointmentDateTimeDto(UpdateAppointmentDateTimeDto updateAppointmentDateTimeDto) =>
            ValidateAppointmentId(updateAppointmentDateTimeDto.AppointmentId) &&
            ValidateAppointmentDate(updateAppointmentDateTimeDto.AppointmentDate) &&
            ValidateAppointmentTime(updateAppointmentDateTimeDto.AppointmentTime);

        public static bool ValidateUpdateAppointmentStatusDto(UpdateAppointmentStatusDto updateAppointmentStatusDto) =>
            ValidateAppointmentId(updateAppointmentStatusDto.AppointmentId) &&
            updateAppointmentStatusDto.Status >= 1 && updateAppointmentStatusDto.Status <= 4;

        public static bool ValidateRescheduleAppointmentDto(RescheduleAppointmentDto rescheduleAppointmentDto) =>
            ValidateAppointmentId(rescheduleAppointmentDto.AppointmentId) &&
            ValidateAppointmentDate(rescheduleAppointmentDto.NewAppointmentDate) &&
            ValidateAppointmentTime(rescheduleAppointmentDto.NewAppointmentTime);
    }
}