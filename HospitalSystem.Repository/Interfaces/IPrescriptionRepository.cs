using HospitalSystem.Data.Entities;

namespace HospitalSystem.Repository.Interfaces
{
    public interface IPrescriptionRepository
    {
        public Task<bool> AddNewPrescriptionAsync(Prescription prescription);

        public Task<Prescription> GetPrescriptionByPrescriptionIdAsync(int prescriptionId);

        public Task<Prescription> GetPrescriptionByAppointmentIdAsync(int appointmentId);
    }
}
