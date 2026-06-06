using HospitalSystem.DTOs.Prescriptions;

namespace HospitalSystem.Service.Interfaces
{
    public interface IPrescriptionService
    {
        public Task<bool> AddNewPrescriptionAsync(AddPrescriptionDto addPrescriptionDto);

        public Task<PrescriptionDto> GetPrescriptionByPrescriptionIdAsync(int prescriptionId);

        public Task<PrescriptionDto> GetPrescriptionByAppointmentIdAsync(int appointmentId);
    }
}
