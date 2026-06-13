using HospitalSystem.Data.Entities;
using HospitalSystem.Data.Data;
using Microsoft.EntityFrameworkCore;
using HospitalSystem.Repository.Interfaces;

namespace HospitalSystem.Repository.Classes
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly HospitalSystemContext _context;

        public PrescriptionRepository(HospitalSystemContext context) => this._context = context;

        public async Task<bool> AddNewPrescriptionAsync(Prescription prescription)
        {
            await this._context.Prescriptions.AddAsync(prescription);
            return await this._context.SaveChangesAsync() > 0;
        }

        public async Task<Prescription> GetPrescriptionByPrescriptionIdAsync(int prescriptionId) =>
            await _context.Prescriptions.FindAsync(prescriptionId);

        public async Task<Prescription> GetPrescriptionByAppointmentIdAsync(int appointmentId) =>
            await _context.Prescriptions.SingleOrDefaultAsync(p => p.AppointmentId == appointmentId);
    }
}
