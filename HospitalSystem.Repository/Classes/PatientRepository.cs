using HospitalSystem.API.Models;
using HospitalSystem.Data.Data;
using HospitalSystem.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Repository.Classes
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HospitalSystemContext _context;

        // AllPatientsView
        public PatientRepository(HospitalSystemContext context) => this._context = context;
        public async Task<List<Patient>> getAllPatientsAsync() =>
            await this._context.Patients
            .SelectAsync

    }
}
