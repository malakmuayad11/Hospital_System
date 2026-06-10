using HospitalSystem.API.Models;
using HospitalSystem.Infrastructure.DTOs.Consultations;
using HospitalSystem.Repository.Interfaces;
using HospitalSystem.Service.Interfaces;

namespace HospitalSystem.Service.Classes
{
    public class ConsultationService : IConsultationService
    {
        private readonly IConsultationRepository _consultationRepository;

        public ConsultationService(IConsultationRepository consultationRepository) =>
            _consultationRepository = consultationRepository;

        public Task<List<ConsultationDto>> GetAllConsultationsAsync() =>
            _consultationRepository.GetAllConsultationsAsync();

        public async Task<FindConsultationDto> FindAsync(int consultationId)
        {
            Consultation consultation = await _consultationRepository.FindAsync(consultationId);

            if (consultation is null)
                return null;

            return new FindConsultationDto
            {
                ConsultationId = consultationId,
                ConsultationName = consultation.ConsultationName,
                ConsultationPrice = consultation.ConsultationFee,
                Specialty = consultation.Specialty
            };
        }

        public async Task<List<string>> GetAllSpecialitiesAsync() =>
            await _consultationRepository.GetAllSpecialitiesAsync();
    }
}
