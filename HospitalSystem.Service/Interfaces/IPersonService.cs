using HospitalSystem.DTOs.People;

namespace HospitalSystem.Service.Interfaces
{
    public interface IPersonService
    {
        public Task<int?> AddNewPersonAsync(AddPersonDto addPersonDto);

        public Task<PersonDto> FindAsync(int personId);

        public Task<bool?> UpdateAsync(UpdatePersonDto updatePersonDto);
    }
}
