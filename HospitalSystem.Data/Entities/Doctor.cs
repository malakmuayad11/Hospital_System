namespace HospitalSystem.Data.Entities;
public partial class Doctor
{
    public int DoctorId { get; set; }

    public int PersonId { get; set; }

    public byte StartWorkDay { get; set; }

    public byte EndWorkDay { get; set; }

    public TimeOnly StartWorkHour { get; set; }

    public TimeOnly EndWorkHour { get; set; }

    public int ConsultationId { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Consultation Consultation { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;

    public virtual User? User { get; set; }
}
