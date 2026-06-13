using HospitalSystem.Repository.Interfaces;
using HospitalSystem.Service.Interfaces;
using HospitalSystem.Data.Entities;
using HospitalSystem.Infrastructure.DTOs.MedicalRecords;

namespace HospitalSystem.Service.Classes
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository) =>
            this._medicalRecordRepository = medicalRecordRepository;

        public async Task<int?> AddNewMedicalRecordAsync(AddMedicalRecordDto addMedicalRecordDto) =>
            await _medicalRecordRepository.AddNewMedicalRecordAsync(new MedicalRecord
            {
                AppointmentId = addMedicalRecordDto.AppointmentId,
                Symptoms = addMedicalRecordDto.Symptoms,
                Diagnosis = addMedicalRecordDto.Diagnosis,
                MedicalRecordNotes = addMedicalRecordDto.MedicalRecordNotes
            });

        public async Task<List<MedicalRecordDto>> GetAllMedicalRecordsAsync() =>
            await _medicalRecordRepository.GetAllMedicalRecordsAsync();

        public async Task<MedicalRecordDto> FindAsync(int medicalRecordId)
        {
            MedicalRecord medicalRecord = await _medicalRecordRepository.FindAsync(medicalRecordId);
            
            if (medicalRecord == null)
                return null;

            return new MedicalRecordDto
            {
                MedicalRecordId = medicalRecord.MedicalRecordId,
                AppointmentId = medicalRecord.AppointmentId,
                Symptoms = medicalRecord.Symptoms,
                Diagnosis = medicalRecord.Diagnosis,
                MedicalRecordNotes = medicalRecord.MedicalRecordNotes
            };
        }

        public async Task<MedicalRecordDto> FindByAppointmentIdAsync(int appointmentId)
        {
            MedicalRecord medicalRecord = await _medicalRecordRepository.FindByAppointmentIdAsync(appointmentId);
            
            if (medicalRecord == null)
                return null;

            return new MedicalRecordDto
            {
                MedicalRecordId = medicalRecord.MedicalRecordId,
                AppointmentId = medicalRecord.AppointmentId,
                Symptoms = medicalRecord.Symptoms,
                Diagnosis = medicalRecord.Diagnosis,
                MedicalRecordNotes = medicalRecord.MedicalRecordNotes
            };
        }
    }
}
