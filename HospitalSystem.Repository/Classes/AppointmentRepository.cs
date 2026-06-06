using HospitalSystem.API.Models;
using HospitalSystem.Data.Data;
using HospitalSystem.DTOs.Appointments;
using HospitalSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Repository.Classes
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HospitalSystemContext _context;

        public AppointmentRepository(HospitalSystemContext context) => _context = context;

        public async Task<int> CountAsync() =>
            await _context.Appointments
            .CountAsync();

        public async Task<List<AppointmentDto>> GetAllAppointmentsAsync() =>
            await _context.Appointments
            .Select(a => new AppointmentDto
            {
                AppointmentId = a.AppointmentId,
                PatientName = a.Patient.Person.FirstName + " " + a.Patient.Person.LastName,
                NationalNo = a.Patient.NationalNo,
                DoctorName = a.Doctor.Person.FirstName + " " + a.Doctor.Person.LastName,
                AppointmentDate = a.AppointmentDate,
                AppointmentTime = a.AppointmentTime,
                Status = a.Status
            })
            .AsNoTracking()
            .ToListAsync();

        public async Task<List<AppointmentDto>> GetTodaysAppointmentsAsync() =>
            await _context.Appointments
            .Select(a => new AppointmentDto
            {
                AppointmentId = a.AppointmentId,
                PatientName = a.Patient.Person.FirstName + " " + a.Patient.Person.LastName,
                NationalNo = a.Patient.NationalNo,
                DoctorName = a.Doctor.Person.FirstName + " " + a.Doctor.Person.LastName,
                AppointmentDate = a.AppointmentDate,
                AppointmentTime = a.AppointmentTime,
                Status = a.Status
            })
            .Where(a => a.AppointmentDate == DateOnly.FromDateTime(DateTime.Today))
            .AsNoTracking()
            .ToListAsync();

        public async Task<int?> AddNewAppointmentAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);

            if (_context.SaveChanges() < 1) return null;

            return appointment.AppointmentId;
        }

        public async Task<bool?> UpdateAppointmentDateTimeAsync(int appointmentId,
            DateOnly appointmentDate,
            TimeOnly appointmentTime)
        {
            Appointment appointment = await this.FindAsync(appointmentId);

            if(appointment == null)
                return null;

            appointment.AppointmentDate = appointmentDate;
            appointment.AppointmentTime = appointmentTime;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool?> UpdateAppointmentStatusAsync(int appointmentId, byte newStatus)
        {
            Appointment appointment = await this.FindAsync(appointmentId);

            if(appointment == null)
                return null;

            appointment.Status = newStatus;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Appointment> FindAsync(int appointmentId) =>
            await _context.Appointments
            .FindAsync(appointmentId);

        public Task<bool?> CancelAppointmentAsync(int appointmentId) =>
            this.UpdateAppointmentStatusAsync(appointmentId, 3);

        public async Task<bool?> RescheduleAppointmentAsync(int appointmentId, DateOnly newDate, TimeOnly newTime)
        {
            Appointment appointment = await this.FindAsync(appointmentId);
            if (appointment == null)
                return null;

            appointment.AppointmentDate = newDate;
            appointment.AppointmentTime = newTime;
            appointment.Status = 2;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool?> HasAppointmentMedicalRecordAsync(int appointmentId)
        {
            if(!await _context.Appointments.AnyAsync(a => a.AppointmentId == appointmentId))
                return null;

            return await _context.MedicalRecords
            .AnyAsync(mr => mr.AppointmentId == appointmentId);
        }
    }
}
