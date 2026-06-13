namespace HospitalSystem.Data.Entities;

public partial class SystemSetting
{
    public int Id { get; set; }
    public int? CurrentUserId { get; set; }

    public virtual User? CurrentUser { get; set; }
}
