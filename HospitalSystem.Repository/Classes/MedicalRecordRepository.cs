using HospitalSystem.Data.Entities;
using HospitalSystem.Data.Data;
using HospitalSystem.Infrastructure.DTOs.MedicalRecords;
using HospitalSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Repository.Classes
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly HospitalSystemContext _context;
        public MedicalRecordRepository(HospitalSystemContext context) => _context = context;

        public async Task<int?> AddNewMedicalRecordAsync(MedicalRecord medicalRecord)
        {
            await _context.MedicalRecords.AddAsync(medicalRecord);

            if (await _context.SaveChangesAsync() < 1) return null;

            return medicalRecord.MedicalRecordId;
        }

        public async Task<List<MedicalRecordDto>> GetAllMedicalRecordsAsync() =>
            await _context.MedicalRecords
            .Select(mr => new MedicalRecordDto
            {
                MedicalRecordId = mr.MedicalRecordId,
                AppointmentId = mr.AppointmentId,
                Symptoms = mr.Symptoms,
                Diagnosis = mr.Diagnosis,
                MedicalRecordNotes = mr.MedicalRecordNotes ?? "No notes"
            })
            .AsNoTracking()
            .ToListAsync();

        public async Task<MedicalRecord> FindAsync(int medicalRecordId) =>
            await _context.MedicalRecords
            .FindAsync(medicalRecordId);

        public async Task<MedicalRecord> FindByAppointmentIdAsync(int appointmentId) =>
            await _context.MedicalRecords
            .AsNoTracking()
            .FirstOrDefaultAsync(mr => mr.AppointmentId == appointmentId);
    }
}
