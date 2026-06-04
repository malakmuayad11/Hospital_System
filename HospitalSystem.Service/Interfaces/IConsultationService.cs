using HospitalSystem.DTOs.Consultations;

namespace HospitalSystem.Service.Interfaces
{
    public interface IConsultationService
    {
        public Task<List<ConsultationDto>> GetAllConsultationsAsync();
        public Task<FindConsultationDto> FindAsync(int consultationId);
        public Task<List<string>> GetAllSpecialitiesAsync();
    }
}
