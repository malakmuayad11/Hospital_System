using HospitalSystem.API.Models;
using HospitalSystem.Data.Data;
using HospitalSystem.DTOs.Consultations;
using HospitalSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Repository.Classes
{
    public class ConsultationRepository : IConsultationRepository
    {
        private readonly HospitalSystemContext _context;

        public ConsultationRepository(HospitalSystemContext context) => _context = context;

        public async Task<List<ConsultationDto>> GetAllConsultationsAsync() =>
            await _context.Consultations
            .Select(c => new ConsultationDto
            {
                ConsultationId = c.ConsultationId,
                ConsultationName = c.ConsultationName,
                ConsultationPrice = c.ConsultationFee
            })
            .AsNoTracking()
            .ToListAsync();

        public async Task<Consultation> FindAsync(int consultationId) =>
            await _context.Consultations.FindAsync(consultationId);

        public async Task<List<string>> GetAllSpecialitiesAsync() =>
            await _context.Consultations
            .Select(c => c.Specialty)
            .AsNoTracking()
            .ToListAsync();
    }
}
