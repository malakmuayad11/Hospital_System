using HospitalSystem.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Service
{
    public interface IPrescriptionService
    {
        public Task<bool> AddNewPrescriptionAsync(AddPrescriptionDto addPrescriptionDto);
    }
}
