using HospitalSystem.API.Models;
using HospitalSystem.Data.Data;
using HospitalSystem.DTOs.Patients;
using HospitalSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Repository.Classes
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HospitalSystemContext _context;

        public PatientRepository(HospitalSystemContext context) => this._context = context;

        public async Task<List<PatientDto>> GetAllPatientsAsync() =>
            await _context.Patients
            .Select(p => new PatientDto
            {
                PatientId = p.PatientId,
                FullName = p.Person.FirstName + " " + p.Person.LastName,
                NationalNo = p.NationalNo,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Person.Gender == 1 ? "Male" : "Female",
                EmergencyContact = p.EmergencyContact
            })
            .AsNoTracking()
            .ToListAsync();

        public async Task<int?> AddNewPatientAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);

            if (_context.SaveChanges() < 1)
                return null;

            return patient.PatientId;
        }

        public async Task<bool?> UpdatePatientAsync(Patient updatedPatient)
        {
            Patient patient = await this.FindAsync(updatedPatient.PatientId);

            if (patient == null)
                return false;

            patient.NationalNo = updatedPatient.NationalNo;
            patient.DateOfBirth = updatedPatient.DateOfBirth;
            patient.Address = updatedPatient.Address;
            patient.EmergencyContact = updatedPatient.EmergencyContact;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Patient> FindAsync(int patientId) =>
            await _context.Patients
            .Include(p => p.Person)
            .FirstOrDefaultAsync(p => p.PatientId == patientId);

        public async Task<Patient> FindAsync(string nationalNo) =>
            await _context.Patients
            .Include(p => p.Person)
            .FirstOrDefaultAsync(p => p.NationalNo == nationalNo);

        public async Task<bool?> DeletePatientAsync(int patientId)
        {
            Patient patient = await this.FindAsync(patientId);

            if(patient is null)
                return false;

           _context.Patients.Remove(patient);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DoesNationalNoExistAsync(string nationalNo) =>
            await _context.Patients
            .AnyAsync(p => p.NationalNo == nationalNo);

        private async Task<bool> _DoesPatientExistAsync(int patientId) =>
            await _context.Patients
            .AnyAsync(p => p.PatientId == patientId);

        public async Task<bool?> HasPatientMedicalRecordsAsync(int patientId)
        {
            if (!await this._DoesPatientExistAsync(patientId))
                return null;

            return await _context.PatientsMedicalRecordsAndPrescriptions
                .AnyAsync(p => p.PatientId == patientId);
        }

        public async Task<bool?> HasPatientPrescriptionsAsync(int patientId)
        {
            if (!await this._DoesPatientExistAsync(patientId))
                return null;

            return await _context.PatientsMedicalRecordsAndPrescriptions
                .AnyAsync(p => p.PatientId == patientId && p.PrescriptionId != null);
        }

        public async Task<bool?> HasPatientAppointmentAtAsync(int patientId,
            DateOnly appointmentDate, TimeOnly appointmentTime)
        {
            if (!await this._DoesPatientExistAsync(patientId))
                return null;

            return await _context.Appointments
            .AnyAsync(a =>
                  a.PatientId == patientId &&
                  a.AppointmentDate == appointmentDate &&
                  a.AppointmentTime == appointmentTime
            );
        }

    }
}
