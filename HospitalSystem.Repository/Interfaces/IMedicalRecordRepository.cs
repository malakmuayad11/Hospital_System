using HospitalSystem.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Repository.Interfaces
{
    public interface IMedicalRecordRepository
    {
        public Task<bool> AddNewMedicalRecordAsync(MedicalRecord medicalRecord);
    }
}
