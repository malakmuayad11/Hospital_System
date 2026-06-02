using HospitalSystem.API.Models;
using HospitalSystem.Data.Data;
using HospitalSystem.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Repository.Classes
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly HospitalSystemContext _context;
        public MedicalRecordRepository(HospitalSystemContext context) => _context = context;

        public async Task<bool> AddNewMedicalRecordAsync(MedicalRecord medicalRecord)
        {
            await _context.MedicalRecords.AddAsync(medicalRecord);
            return await _context.SaveChangesAsync() == 1;
        }
    }
}
