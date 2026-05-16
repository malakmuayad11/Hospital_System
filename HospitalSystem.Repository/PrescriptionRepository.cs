using HospitalSystem.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalSystem.Data;
using HospitalSystem.Data.Data;

namespace HospitalSystem.Repository
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly HospitalSystemContext _context;

        public PrescriptionRepository(HospitalSystemContext context) => this._context = context;

        public async Task<bool> AddNewPrescriptionAsync(Prescription prescription)
        {
             await this._context.Prescriptions.AddAsync(prescription);
            return await this._context.SaveChangesAsync() == 1;
        }
    }
}
