using HospitalSystem.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Service.Interfaces
{
    public interface IPrescriptionService
    {
        public Task<bool> AddNewPrescriptionAsync(AddPrescriptionDto addPrescriptionDto);

        public Task<PrescriptionDto> GetPrescriptionByPrescriptionIdAsync(int PrescriptionId);

        public Task<PrescriptionDto> GetPrescriptionByAppointmentIdAsync(int AppointmentId);
    }
}
