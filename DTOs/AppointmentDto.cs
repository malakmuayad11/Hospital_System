using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.DTOs
{
    public class AppointmentDto
    {
        public int AppointmentId {  get; set; }
    
        public string PatientName { get; set; }
        public string NationalNo { get; set; }
        public string DoctorName { get; set;  }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly AppointmentTime { get; set; }
        public byte Status { get; set; }

    
    }
}
