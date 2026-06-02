using HospitalSystem.API.Models;
using HospitalSystem.DTOs.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Service.Interfaces
{
    public interface IMedicalRecordService
    {
        public Task<bool> AddNewMedicalRecordAsync(AddMedicalRecordDto addMedicalRecordDto);
    }
}
