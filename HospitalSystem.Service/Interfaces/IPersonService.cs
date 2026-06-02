using HospitalSystem.DTOs.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Service.Interfaces
{
    public interface IPersonService
    {
        public Task<int?> AddNewPersonAsync(AddPersonDto addPersonDto);

        public Task<PersonDto> FindAsync(int personId);

        public Task<bool?> updateAsync(UpdatePersonDto updatePersonDto);
    }
}
