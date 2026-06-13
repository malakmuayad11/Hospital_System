using HospitalSystem.API.Models;
using HospitalSystem.Repository.Interfaces;
using HospitalSystem.Service.Interfaces;
using HospitalSystem.Infrastructure.DTOs.People;

namespace HospitalSystem.Service.Classes
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository) => this._personRepository = personRepository;

        public async Task<int?> AddNewPersonAsync(AddPersonDto addPersonDto)
        {
            Person person = new Person
            {
                FirstName = addPersonDto.FirstName,
                LastName = addPersonDto.LastName,
                Phone = addPersonDto.Phone,
                Email = addPersonDto.Email,
                Gender = addPersonDto.Gender
            };
            return await this._personRepository.AddNewPersonAsync(person);
        }

        public async Task<PersonDto> FindAsync(int personId)
        {
            Person person = await _personRepository.FindAsync(personId);

            if (person == null)
                return null;

            return new PersonDto
            {
                PersonId = personId,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Phone = person.Phone,
                Email = person.Email,
                Gender = person.Gender
            };
        }

        public async Task<bool?> UpdateAsync(UpdatePersonDto updatePersonDto) =>
            await _personRepository.UpdateAsync(new Person
            {
                PersonId = updatePersonDto.PersonId,
                FirstName = updatePersonDto.FirstName,
                LastName = updatePersonDto.LastName,
                Phone = updatePersonDto.Phone,
                Email = updatePersonDto.Email,
                Gender = updatePersonDto.Gender
            });
    }
}
