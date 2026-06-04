using HospitalSystem.API.Models;
using HospitalSystem.Data.Data;
using HospitalSystem.DTOs.Appointments;
using HospitalSystem.DTOs.Doctors;
using HospitalSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Repository.Classes
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HospitalSystemContext _context;

        public DoctorRepository(HospitalSystemContext context) => _context = context;

        public async Task<int> DoctorsCount() => await _context.Doctors.CountAsync();

        public async Task<bool> AddNewDoctorAsync(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            return await _context.SaveChangesAsync() == 1;
        }

        public async Task<bool?> UpdateDoctorAsync(Doctor updatedDoctor)
        {
            Doctor doctor = await _context.Doctors.FindAsync(updatedDoctor.DoctorId);
            if (doctor == null)
                return null;

            // Update the doctor's properties
            doctor.StartWorkDay = updatedDoctor.StartWorkDay;
            doctor.EndWorkDay = updatedDoctor.EndWorkDay;
            doctor.StartWorkHour = updatedDoctor.StartWorkHour;
            doctor.EndWorkHour = updatedDoctor.EndWorkHour;
            doctor.ConsultationId = updatedDoctor.ConsultationId;

            _context.Doctors.Update(doctor);
            return await _context.SaveChangesAsync() == 1;
        }

        public async Task<Doctor> FindAsync(int doctorId) =>
            await _context.Doctors.FindAsync(doctorId);

        public async Task<Doctor> FindByUserIdAsync(int userId) =>
            await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);

        public async Task<bool?> DeleteDoctorAsync(int doctorId)
        {
            Doctor doctor = await _context.Doctors.FindAsync(doctorId);

            if (doctor == null)
                return null;

            _context.Doctors.Remove(doctor);
            return await _context.SaveChangesAsync() == 1;
        }

        private static string _GetDayOfWeek(byte dayOfWeek)
        {
            return dayOfWeek switch
            {
                0 => "Monday",
                1 => "Tuesday",
                2 => "Wednesday",
                3 => "Thursday",
                4 => "Friday",
                5 => "Saturday",
                6 => "Sunday"
            };
        }

        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            return await _context.Doctors
                .Select(d => new DoctorDto
                {
                    DictorId = d.DoctorId,
                    Name = d.Person.FirstName + " " + d.Person.LastName,
                    StartWorkDay = _GetDayOfWeek(d.StartWorkDay),
                    EndWorkDay = _GetDayOfWeek(d.EndWorkDay),
                    StartWorkHour = d.StartWorkHour,
                    EndWorkHour = d.EndWorkHour,
                    Specialty = d.Consultation.Specialty
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<AppointmentForDoctorDto>> GetTodaysAppointmentsForDoctorAsync(int doctorId)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate == DateOnly.FromDateTime(DateTime.Today))
                .Select(a => new AppointmentForDoctorDto
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.AppointmentTime,
                    ReasonForVisit = a.ReasonForVisit,
                    Status = a.Status
                })
                .AsNoTracking()
                .ToListAsync();
        }

        private async Task<bool> _DoesDoctorExist(int doctorId) =>
            await _context.Doctors.AnyAsync(d => d.DoctorId == doctorId);
        // may be not correct
        public async Task<int?> PatientsCountForDoctorAsync(int doctorId)
        {
            if (!await _DoesDoctorExist(doctorId))
                return null;

            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .Select(a => a.PatientId)
                .Distinct()
                .CountAsync();
        }

        public async Task<int?> AppointmentsCountForDoctorAsync(int doctorId)
        {
            if (!await _DoesDoctorExist(doctorId))
                return null;

            return await _context.Appointments
                .Distinct()
                .CountAsync(a => a.DoctorId == doctorId);
        }

        public async Task<int?> MedicalRecordsCountForDoctorAsync(int doctorId)
        {
            if (!await _DoesDoctorExist(doctorId))
                return null;

            return await _context.MedicalRecords
                .Where(mr => mr.Appointment.DoctorId == doctorId)
                .CountAsync();
        }
    }
}
