using System;
using System.Collections.Generic;

namespace HospitalSystem.API.Models;

public partial class PasswordLog
{
    public int LogId { get; set; }

    public string OldPassword { get; set; } = null!;

    public string NewPassword { get; set; } = null!;

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
