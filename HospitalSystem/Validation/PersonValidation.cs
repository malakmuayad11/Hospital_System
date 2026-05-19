using HospitalSystem.DTOs;

namespace HospitalSystem.API.Validation
{
    public class PersonValidation
    {
        public static bool validatePersonId(int personId) => personId > 0;
        public static bool validateAddPersonDto(AddPersonDto addPersonDto) =>
            !string.IsNullOrEmpty(addPersonDto.FirstName) &&
            !string.IsNullOrEmpty(addPersonDto.LastName) &&
            !string.IsNullOrEmpty(addPersonDto.Phone) &&
            (addPersonDto.Gender == 1 || addPersonDto.Gender == 2);
    }
}
