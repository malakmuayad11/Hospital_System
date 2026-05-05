using System;
using System.Collections.Generic;

namespace HospitalSystem.API.Models;

public partial class SystemSetting
{
    public int? CurrentUserId { get; set; }

    public virtual User? CurrentUser { get; set; }
}
