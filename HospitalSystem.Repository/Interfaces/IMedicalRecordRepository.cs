using HospitalSystem.API.Models;
using HospitalSystem.DTOs.MedicalRecords;
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

        public Task<List<MedicalRecordDto>> GetAllMedicalRecordsAsync();
        
        public Task<MedicalRecord> FindAsync(int medicalRecordId);
        public Task<MedicalRecord> FindByAppointmentIdAsync(int appointmentId);
    }
}
