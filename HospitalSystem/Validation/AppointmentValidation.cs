namespace HospitalSystem.API.Validation
{
    public class AppointmentValidation
    {
        public static bool ValidateAppointmentDate(DateOnly appointmentDate) =>
            appointmentDate >= DateOnly.FromDateTime(DateTime.Today);

        public static bool ValidateAppointmentTime(TimeOnly appointmentTime) =>
            appointmentTime > TimeOnly.FromDateTime(DateTime.Now);
    }
}