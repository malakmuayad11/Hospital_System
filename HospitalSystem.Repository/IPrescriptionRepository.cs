using HospitalSystem.API.Models;
using HospitalSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Repository
{
    public interface IPrescriptionRepository
    {
        public Task<bool> AddNewPrescriptionAsync(Prescription prescription);
    }
}
