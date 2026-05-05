using System;
using System.Collections.Generic;

namespace HospitalSystem.API.Models;

public partial class ErrorLog
{
    public int LogId { get; set; }

    public int ErrorNumber { get; set; }

    public string? ErrorProcedure { get; set; }

    public string ErrorMessage { get; set; } = null!;
}
