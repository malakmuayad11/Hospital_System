using HospitalSystem.API.Models;
using HospitalSystem.DTOs.Consultations;

namespace HospitalSystem.Repository.Interfaces
{
    public interface IConsultationRepository
    {
        public Task<List<ConsultationDto>> GetAllConsultationsAsync();

        public Task<Consultation> FindAsync(int consultationId);

        public Task<List<string>> GetAllSpecialitiesAsync();
    }
}
