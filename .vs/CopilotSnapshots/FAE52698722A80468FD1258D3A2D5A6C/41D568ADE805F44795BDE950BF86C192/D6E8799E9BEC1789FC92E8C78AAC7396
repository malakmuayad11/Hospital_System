using HospitalSystem.API.Models;
using HospitalSystem.Infrastructure.DTOs.Prescriptions;
using HospitalSystem.Repository.Interfaces;
using HospitalSystem.Service.Interfaces;

namespace HospitalSystem.Service.Classes
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _prescriptionRepository;

        public PrescriptionService(IPrescriptionRepository prescriptionRepository) =>
            this._prescriptionRepository = prescriptionRepository;

        public async Task<bool> AddNewPrescriptionAsync(AddPrescriptionDto addPrescriptionDto)
        {
            Prescription prescription = new Prescription
            {
                AppointmentId = addPrescriptionDto.AppointmentId,
                MedicationName = addPrescriptionDto.MedicationName,
                Dosage = addPrescriptionDto.Dosage,
                DurationDays = addPrescriptionDto.DurationDays,
                DurationMonths = addPrescriptionDto.DurationMonths,
            };
            return await _prescriptionRepository.AddNewPrescriptionAsync(prescription);
        }

        public async Task<PrescriptionDto> GetPrescriptionByPrescriptionIdAsync(int PrescriptionId)
        {
            Prescription prescription = await _prescriptionRepository.GetPrescriptionByPrescriptionIdAsync(PrescriptionId);

            if (prescription == null)
                return null;

            return new PrescriptionDto
            {
                PrescriptionId = prescription.PrescriptionId,
                AppointmentId = prescription.AppointmentId,
                MedicationName = prescription.MedicationName,
                Dosage = prescription.Dosage,
                DurationDays = prescription.DurationDays,
                DurationMonths = prescription.DurationMonths
            };
        }
    
        public async Task<PrescriptionDto> GetPrescriptionByAppointmentIdAsync(int AppointmentId)
        {
            Prescription prescription = await _prescriptionRepository.GetPrescriptionByAppointmentIdAsync(AppointmentId);
            
            if (prescription == null)
                return null;

            return new PrescriptionDto
            {
                PrescriptionId = prescription.PrescriptionId,
                AppointmentId = prescription.AppointmentId,
                MedicationName = prescription.MedicationName,
                Dosage = prescription.Dosage,
                DurationDays = prescription.DurationDays,
                DurationMonths = prescription.DurationMonths
            };
        }
    }
}
