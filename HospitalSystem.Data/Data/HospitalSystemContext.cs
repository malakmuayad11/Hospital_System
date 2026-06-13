using HospitalSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HospitalSystem.Data.Data;

public partial class HospitalSystemContext : DbContext
{
    public HospitalSystemContext()
    {
    }

    public HospitalSystemContext(DbContextOptions<HospitalSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Billing> Billings { get; set; }

    public virtual DbSet<Consultation> Consultations { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<PatientsMedicalRecordsAndPrescription> PatientsMedicalRecordsAndPrescriptions { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<SystemSetting> SystemSettings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersTokens> UsersTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
            return;

        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        var config = configBuilder.Build();

        var keyVaultUrl = config["KeyVault:Url"];
        if (!string.IsNullOrWhiteSpace(keyVaultUrl))
        {
            try
            {
                configBuilder.AddAzureKeyVault(new Uri(keyVaultUrl), new Azure.Identity.DefaultAzureCredential());
                config = configBuilder.Build();
            }
            catch
            {
                // If Key Vault cannot be reached, fall back to local configuration.
            }
        }

        var connectionString = config["ConnectionString"]
            ?? config["ConnectionStrings:HospitalDB"];

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA24BB8CF7D");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("trg_AfterInsertAppointment");
                    tb.HasTrigger("trg_AfterUpdateAppointment");
                });

            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.IsReminderSent).HasDefaultValue(false);
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.ReasonForVisit).HasMaxLength(150);
            entity.Property(e => e.Status).HasComment("1- Scheduled. 2- ");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Docto__4316F928");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Patie__440B1D61");
        });

        modelBuilder.Entity<Billing>(entity =>
        {
            entity.HasKey(e => e.BillingId).HasName("PK__Billings__F1656D13B47DD134");

            entity.Property(e => e.BillingId).HasColumnName("BillingID");
            entity.Property(e => e.AdditionalCharges).HasColumnType("smallmoney");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.ConsultationFee).HasColumnType("smallmoney");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Billings)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Billings__Appoin__4CA06362");
        });

        modelBuilder.Entity<Consultation>(entity =>
        {
            entity.HasKey(e => e.ConsultationId).HasName("PK__Consulta__5D014A78E547F03A");

            entity.Property(e => e.ConsultationId).HasColumnName("ConsultationID");
            entity.Property(e => e.ConsultationFee).HasColumnType("smallmoney");
            entity.Property(e => e.ConsultationName).HasMaxLength(100);
            entity.Property(e => e.Specialty).HasMaxLength(150);
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("PK__Doctors__2DC00EDF3DF2714E");

            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.ConsultationId).HasColumnName("ConsultationID");
            entity.Property(e => e.PersonId).HasColumnName("PersonID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Consultation).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.ConsultationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Doctors__Consult__3F466844");

            entity.HasOne(d => d.Person).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Doctors__PersonI__403A8C7D");

            entity.HasOne(d => d.User).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Doctors__UserID__0B91BA14");
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(e => e.MedicalRecordId).HasName("PK__Treatmen__1A57B711CB5F0A0D");

            entity.ToTable(tb => tb.HasTrigger("trg_AfterInsertMedicalRecord"));

            entity.Property(e => e.MedicalRecordId).HasColumnName("MedicalRecordID");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.Diagnosis).HasMaxLength(150);
            entity.Property(e => e.Symptoms).HasMaxLength(255);

            entity.HasOne(d => d.Appointment).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Treatment__Appoi__49C3F6B7");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patients__970EC346C468B3CC");

            entity.HasIndex(e => e.NationalNo, "UQ__Patients__E9AA1A651ECDC414").IsUnique();

            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.EmergencyContact).HasMaxLength(20);
            entity.Property(e => e.NationalNo).HasMaxLength(20);
            entity.Property(e => e.PersonId).HasColumnName("PersonID");

            entity.HasOne(d => d.Person).WithMany(p => p.Patients)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Patients__Person__3C69FB99");
        });

        modelBuilder.Entity<PatientsMedicalRecordsAndPrescription>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("PatientsMedicalRecordsAndPrescriptions");

            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.MedicalRecordId).HasColumnName("MedicalRecordID");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.PrescriptionId).HasColumnName("PrescriptionID");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PK__People__AA2FFB8550C51863");

            entity.Property(e => e.PersonId).HasColumnName("PersonID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.PrescriptionId).HasName("PK__Prescrip__401308124B98D52C");

            entity.ToTable(tb => tb.HasTrigger("trg_AfterInsertPrescription"));

            entity.Property(e => e.PrescriptionId).HasColumnName("PrescriptionID");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.Dosage).HasMaxLength(50);
            entity.Property(e => e.MedicationName).HasMaxLength(100);

            entity.HasOne(d => d.Appointment).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prescript__Appoi__46E78A0C");
        });

        modelBuilder.Entity<SystemSetting>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.CurrentUserId).HasColumnName("CurrentUserID");

            entity.HasOne(d => d.CurrentUser).WithMany()
                .HasForeignKey(d => d.CurrentUserId)
                .HasConstraintName("FK__SystemSet__Curre__467D75B8");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACD56994E9");

            entity.ToTable(tb => tb.HasTrigger("trg_AfterUpdateUser"));

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.LastLoginDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.Password).HasMaxLength(64);
            entity.Property(e => e.Role).HasComment("1- Admin, 2- Receptionist, 3- Doctor");
            entity.Property(e => e.Username).HasMaxLength(50);
        
        });
        modelBuilder.Entity<UsersTokens>(entity =>
        {
            entity.HasKey(e => e.TokenId)
                  .HasName("PK_Users_Tokens");

            entity.Property(e => e.TokenId)
                  .HasColumnName("TokenID");

            entity.Property(e => e.UserId)
                  .HasColumnName("UserID");

            entity.Property(e => e.RefreshTokenHash)
                  .HasMaxLength(100);

            entity.Property(e => e.RefreshTokenExpiresAt)
                  .HasColumnType("datetime2");

            entity.Property(e => e.RefreshTokenRevokedAt)
                  .HasColumnType("datetime2");

            entity.HasOne(d => d.User)
                  .WithMany(p => p.UsersTokens)
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("FK_Tokens_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
