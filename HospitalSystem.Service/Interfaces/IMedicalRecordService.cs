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

        public Task<List<MedicalRecordDto>> GetAllMedicalRecordsAsync();

        public Task<MedicalRecordDto> FindAsync(int medicalRecordId);

        public Task<MedicalRecordDto> FindByAppointmentIdAsync(int appointmentId);
    }
}
