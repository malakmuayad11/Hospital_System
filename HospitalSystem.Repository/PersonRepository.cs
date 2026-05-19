using HospitalSystem.API.Models;
using HospitalSystem.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly HospitalSystemContext _context;

        public PersonRepository(HospitalSystemContext context) => this._context = context;

        public async Task<int?> AddNewPersonAsync(Person person)
        {
            await _context.People.AddAsync(person);
            if (await _context.SaveChangesAsync() != 1)
                return null;
            return person.PersonId;
        }

        public async Task<Person> FindAsync(int personId) =>
            await _context.People.FindAsync(personId);
    }
}
