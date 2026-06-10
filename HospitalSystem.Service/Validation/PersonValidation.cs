using HospitalSystem.Infrastructure.DTOs.People;

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

        public static bool validateUpdatePersonDto(UpdatePersonDto updatePersonDto) =>
            validatePersonId(updatePersonDto.PersonId) &&
            !string.IsNullOrEmpty(updatePersonDto.FirstName) &&
            !string.IsNullOrEmpty(updatePersonDto.LastName) &&
            !string.IsNullOrEmpty(updatePersonDto.Phone) &&
            (updatePersonDto.Gender == 1 || updatePersonDto.Gender == 2);
    }
}
