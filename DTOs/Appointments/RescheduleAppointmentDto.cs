using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.DTOs.Appointments
{
    public class RescheduleAppointmentDto
    {
        public int AppointmentId { get; set; }

        public DateOnly NewAppointmentDate { get; set; }

        public TimeOnly NewAppointmentTime { get; set; }
    }
}
