using System;
using System.Collections.Generic;

namespace HospitalSystem.API.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    /// <summary>
    /// 1- Admin, 2- Receptionist, 3- Doctor
    /// </summary>
    public byte Role { get; set; }

    public int Permissions { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public virtual ICollection<AuditTrail> AuditTrails { get; set; } = new List<AuditTrail>();


    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    public virtual ICollection<PasswordLog> PasswordLogs { get; set; } = new List<PasswordLog>();
}
