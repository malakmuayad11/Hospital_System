using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalSystem.API.Models;
using HospitalSystem.Data;

namespace HospitalSystem.Repository.Interfaces
{
    public interface IPersonRepository
    {
        public Task<int?> AddNewPersonAsync(Person person);

        public Task<Person> FindAsync(int personId);

        public Task<bool?> UpdateAsync(Person person);
    }
}
