using HospitalSystem.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Service
{
    public interface IPersonService
    {
        public Task<int?> AddNewPersonAsync(AddPersonDto addPersonDto);

        public Task<PersonDto> FindAsync(int personId);
    }
}
