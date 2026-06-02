using HospitalSystem.Repository.Interfaces;
using HospitalSystem.Service.Interfaces;
using HospitalSystem.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalSystem.DTOs.MedicalRecords;

namespace HospitalSystem.Service.Classes
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository) =>
            this._medicalRecordRepository = medicalRecordRepository;

        public async Task<bool> AddNewMedicalRecordAsync(AddMedicalRecordDto addMedicalRecordDto) =>
            await _medicalRecordRepository.AddNewMedicalRecordAsync(new MedicalRecord
            {
                AppointmentId = addMedicalRecordDto.AppointmentId,
                Symptoms = addMedicalRecordDto.Symptoms,
                Diagnosis = addMedicalRecordDto.Diagnosis,
                MedicalRecordNotes = addMedicalRecordDto.MedicalRecordNotes
            });
    }
}
