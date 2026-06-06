using HospitalSystem.API.Models;

namespace HospitalSystem.Repository.Interfaces
{
    public interface IPersonRepository
    {
        public Task<int?> AddNewPersonAsync(Person person);

        public Task<Person> FindAsync(int personId);

        public Task<bool?> UpdateAsync(Person person);
    }
}
