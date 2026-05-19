using HospitalSystem.API.Models;
using HospitalSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Repository.Interfaces
{
    public interface IPrescriptionRepository
    {
        public Task<bool> AddNewPrescriptionAsync(Prescription prescription);

        public Task<Prescription> GetPrescriptionByPrescriptionIdAsync(int PrescriptionId);

        public Task<Prescription> GetPrescriptionByAppointmentIdAsync(int AppointmentId);
    }
}
