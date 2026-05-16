using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalSystem.API.Models;
using HospitalSystem.DTOs;
using HospitalSystem.Repository;

namespace HospitalSystem.Service
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _prescriptionRepository;

        public PrescriptionService(IPrescriptionRepository prescriptionRepository) =>
            this._prescriptionRepository =  prescriptionRepository;

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
    }
}
