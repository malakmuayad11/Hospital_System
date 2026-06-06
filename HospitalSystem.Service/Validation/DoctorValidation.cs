using HospitalSystem.DTOs.Doctors;

namespace HospitalSystem.Service.Validation
{
    public class DoctorValidation
    {
        private static bool _ValidateDay(byte Day) => Day >= 1 && Day <= 7;

        private static bool _ValidateHour(TimeOnly Hour) => Hour.Hour >= 0 && Hour.Hour <= 23;

        public static bool ValidateDoctorId(int doctorId) => doctorId > 0;

        public static bool ValidateUserId(int userId) => userId > 0;

        public static bool ValidateAddDoctorDto(AddDoctorDto addDoctorDto) =>
            addDoctorDto.PersonId > 0 &&
            _ValidateDay(addDoctorDto.StartWorkDay) &&
            _ValidateDay(addDoctorDto.EndWorkDay) &&
            _ValidateHour(addDoctorDto.StartWorkHour) &&
            _ValidateHour(addDoctorDto.EndWorkHour) &&
            addDoctorDto.ConsultationId > 0;

        public static bool ValidateUpdateDoctorDto(UpdateDoctorDto updateDoctorDto) =>
            ValidateDoctorId(updateDoctorDto.DoctorId) &&
            _ValidateDay(updateDoctorDto.StartWorkDay) &&
            _ValidateDay(updateDoctorDto.EndWorkDay) &&
            _ValidateHour(updateDoctorDto.StartWorkHour) &&
            _ValidateHour(updateDoctorDto.EndWorkHour) &&
            updateDoctorDto.ConsultationId > 0;
    }
}
