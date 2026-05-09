using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.DTOs
{
    public class ChangePasswordDto
    {
        public int UserId { get; set; }
        public string Password { get; set; } = null!;
    }
}
