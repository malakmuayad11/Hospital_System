using System;
using System.Collections.Generic;
using HospitalSystem.API.Models;
using Microsoft.EntityFrameworkCore;

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

    public virtual DbSet<AllAppointmentsView> AllAppointmentsViews { get; set; }

    public virtual DbSet<AllDoctorsView> AllDoctorsViews { get; set; }

    public virtual DbSet<AllPatientsView> AllPatientsViews { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AppointmentAuditTrail> AppointmentAuditTrails { get; set; }

    public virtual DbSet<AuditTrail> AuditTrails { get; set; }

    public virtual DbSet<Billing> Billings { get; set; }

    public virtual DbSet<Consultation> Consultations { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }

    public virtual DbSet<MedicalRecordAuditTrail> MedicalRecordAuditTrails { get; set; }

    public virtual DbSet<PasswordLog> PasswordLogs { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<PatientsMedicalRecordsAndPrescription> PatientsMedicalRecordsAndPrescriptions { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<PrescriptionAuditTrail> PrescriptionAuditTrails { get; set; }

    public virtual DbSet<SystemSetting> SystemSettings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=Hospital_System;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AllAppointmentsView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("AllAppointmentsView");

            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.DoctorName).HasMaxLength(100);
            entity.Property(e => e.NationalNo).HasMaxLength(20);
            entity.Property(e => e.PatientName).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(11)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AllDoctorsView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("AllDoctorsView");

            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.EndWorkDay).HasMaxLength(9);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Specialty).HasMaxLength(150);
            entity.Property(e => e.StartWorkDay).HasMaxLength(9);
        });

        modelBuilder.Entity<AllPatientsView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("AllPatientsView");

            entity.Property(e => e.EmergencyContact).HasMaxLength(20);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.NationalNo).HasMaxLength(20);
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
        });

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

        modelBuilder.Entity<AppointmentAuditTrail>(entity =>
        {
            entity.HasKey(e => e.AppointmentAuditId).HasName("PK__Appointm__900C51F6415C540F");

            entity.Property(e => e.AppointmentAuditId).HasColumnName("AppointmentAuditID");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.AuditId).HasColumnName("AuditID");

            entity.HasOne(d => d.Appointment).WithMany(p => p.AppointmentAuditTrails)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Appoi__5EBF139D");

            entity.HasOne(d => d.Audit).WithMany(p => p.AppointmentAuditTrails)
                .HasForeignKey(d => d.AuditId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Audit__5FB337D6");
        });

        modelBuilder.Entity<AuditTrail>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PK__AuditTra__A17F23B84082D50B");

            entity.Property(e => e.AuditId).HasColumnName("AuditID");
            entity.Property(e => e.ActionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.ActionPerformed).HasComment("1- Add, 2- Update, 3- Delete");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.AuditTrails)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AuditTrai__UserI__534D60F1");
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

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__ErrorLog__5E5499A89AE3F999");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.ErrorProcedure).HasMaxLength(200);
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

        modelBuilder.Entity<MedicalRecordAuditTrail>(entity =>
        {
            entity.HasKey(e => e.TreatmentAuditId).HasName("PK__Treatmen__20E521DE94EAC5E4");

            entity.Property(e => e.TreatmentAuditId).HasColumnName("TreatmentAuditID");
            entity.Property(e => e.AuditId).HasColumnName("AuditID");
            entity.Property(e => e.MedicalRecordId).HasColumnName("MedicalRecordID");

            entity.HasOne(d => d.Audit).WithMany(p => p.MedicalRecordAuditTrails)
                .HasForeignKey(d => d.AuditId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Treatment__Audit__5BE2A6F2");

            entity.HasOne(d => d.MedicalRecord).WithMany(p => p.MedicalRecordAuditTrails)
                .HasForeignKey(d => d.MedicalRecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Treatment__Treat__5AEE82B9");
        });

        modelBuilder.Entity<PasswordLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Password__5E5499A8A46BC47E");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.NewPassword).HasMaxLength(64);
            entity.Property(e => e.OldPassword).HasMaxLength(64);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.PasswordLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PasswordL__UserI__0E6E26BF");
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

        modelBuilder.Entity<PrescriptionAuditTrail>(entity =>
        {
            entity.HasKey(e => e.PrescriptionAuditId).HasName("PK__Prescrip__212FF37A2DF2EB7D");

            entity.Property(e => e.PrescriptionAuditId).HasColumnName("PrescriptionAuditID");
            entity.Property(e => e.AuditId).HasColumnName("AuditID");
            entity.Property(e => e.PrescriptionId).HasColumnName("PrescriptionID");

            entity.HasOne(d => d.Audit).WithMany(p => p.PrescriptionAuditTrails)
                .HasForeignKey(d => d.AuditId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prescript__Audit__6383C8BA");

            entity.HasOne(d => d.Prescription).WithMany(p => p.PrescriptionAuditTrails)
                .HasForeignKey(d => d.PrescriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prescript__Presc__628FA481");
        });

        modelBuilder.Entity<SystemSetting>(entity =>
        {
            entity.HasNoKey();

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
