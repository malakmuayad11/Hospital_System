using HospitalSystem.Data.Entities;
using HospitalSystem.Data.Data;
using HospitalSystem.Repository.Interfaces;

namespace HospitalSystem.Repository.Classes
{
    public class PersonRepository : IPersonRepository
    {
        private readonly HospitalSystemContext _context;

        public PersonRepository(HospitalSystemContext context) => this._context = context;

        public async Task<int?> AddNewPersonAsync(Person person)
        {
            await _context.People.AddAsync(person);
            if (await _context.SaveChangesAsync() < 1)
                return null;
            return person.PersonId;
        }

        public async Task<Person> FindAsync(int personId) =>
            await _context.People.FindAsync(personId);

        public async Task<bool?> UpdateAsync(Person updatedPerson)
        {
            Person person = await this.FindAsync(updatedPerson.PersonId);

            if (person == null)
                return null;

            person.FirstName = updatedPerson.FirstName;
            person.LastName = updatedPerson.LastName;
            person.Phone = updatedPerson.Phone;
            person.Email = updatedPerson.Email;
            person.Gender = updatedPerson.Gender;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
