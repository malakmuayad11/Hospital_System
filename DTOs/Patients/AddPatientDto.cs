using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.DTOs.Patients
{
    public class AddPatientDto
    {
        public int PersonId { get; set; }
        public string NationalNo { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; }
        public string EmergencyContact { get; set; }
    }
}
