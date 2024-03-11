using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ResourceEntity.Models;

public partial class ResourceMngContext : DbContext
{
    public ResourceMngContext()
    {
    }

    public ResourceMngContext(DbContextOptions<ResourceMngContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Applicant> Applicants { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Interview> Interviews { get; set; }

    public virtual DbSet<PhanQuyen> PhanQuyens { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<RoleDesc> RoleDescs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vacancy> Vacancies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=103.1.209.95;Database=BPO_HRM;user id=wms;password=wms$2023;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Applicant>(entity =>
        {
            entity.ToTable("Applicant");

            entity.Property(e => e.ApplicantId).HasColumnName("ApplicantID");
            entity.Property(e => e.ActiveEmployee)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ApplicantAddress).HasMaxLength(50);
            entity.Property(e => e.ApplicantCv)
                .HasMaxLength(500)
                .HasColumnName("ApplicantCV");
            entity.Property(e => e.ApplicantDob)
                .HasColumnType("date")
                .HasColumnName("ApplicantDOB");
            entity.Property(e => e.ApplicantEmail).HasMaxLength(50);
            entity.Property(e => e.ApplicantName).HasMaxLength(50);
            entity.Property(e => e.ApplicantPhone).HasMaxLength(50);
            entity.Property(e => e.ApplicantPicture).HasMaxLength(500);
            entity.Property(e => e.ApplicantPob)
                .HasMaxLength(50)
                .HasColumnName("ApplicantPOB");
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.VacancyId).HasColumnName("VacancyID");

            entity.HasOne(d => d.Vacancy).WithMany(p => p.Applicants)
                .HasForeignKey(d => d.VacancyId)
                .HasConstraintName("FK_Applicant_Vacancy");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EmployeeAddress).HasMaxLength(50);
            entity.Property(e => e.EmployeeAvatar).HasMaxLength(500);
            entity.Property(e => e.EmployeeCv)
                .HasMaxLength(500)
                .HasColumnName("EmployeeCV");
            entity.Property(e => e.EmployeeDelete)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.EmployeeDepartment).HasMaxLength(50);
            entity.Property(e => e.EmployeeEmail).HasMaxLength(50);
            entity.Property(e => e.EmployeeGender).HasMaxLength(10);
            entity.Property(e => e.EmployeeName).HasMaxLength(50);
            entity.Property(e => e.EmployeePhone).HasMaxLength(50);
        });

        modelBuilder.Entity<Interview>(entity =>
        {
            entity.ToTable("Interview");

            entity.Property(e => e.InterviewId).HasColumnName("InterviewID");
            entity.Property(e => e.ApplicantId).HasColumnName("ApplicantID");
            entity.Property(e => e.InterviewAddress).HasMaxLength(50);
            entity.Property(e => e.InterviewDescription).HasMaxLength(50);
            entity.Property(e => e.InterviewEd)
                .HasColumnType("datetime")
                .HasColumnName("InterviewED");
            entity.Property(e => e.InterviewResultOne)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.InterviewResultTwo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.InterviewSchedule)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.InterviewSd)
                .HasColumnType("datetime")
                .HasColumnName("InterviewSD");

            entity.HasOne(d => d.Applicant).WithMany(p => p.Interviews)
                .HasForeignKey(d => d.ApplicantId)
                .HasConstraintName("FK_Interview_Applicant");
        });

        modelBuilder.Entity<PhanQuyen>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });

            entity.ToTable("PhanQuyen");

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.RoleId).HasColumnName("roleID");
            entity.Property(e => e.Note)
                .HasMaxLength(50)
                .HasColumnName("note");

            entity.HasOne(d => d.Role).WithMany(p => p.PhanQuyens)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_PhanQuyen_RoleDesc");

            entity.HasOne(d => d.User).WithMany(p => p.PhanQuyens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_PhanQuyen_User");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("Project");

            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.ProjectActive)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ProjectClient).HasMaxLength(50);
            entity.Property(e => e.ProjectDelete)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ProjectDescription).HasColumnType("ntext");
            entity.Property(e => e.ProjectName).HasMaxLength(50);
        });

        modelBuilder.Entity<RoleDesc>(entity =>
        {
            entity.ToTable("RoleDesc");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.Controller).HasMaxLength(50);
            entity.Property(e => e.Link).HasMaxLength(50);
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("roleName");
            entity.Property(e => e.Stt).HasColumnName("STT");
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(50)
                .HasColumnName("fullname");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Vacancy>(entity =>
        {
            entity.ToTable("Vacancy");

            entity.Property(e => e.VacancyId).HasColumnName("VacancyID");
            entity.Property(e => e.VacancyCd)
                .HasColumnType("date")
                .HasColumnName("VacancyCD");
            entity.Property(e => e.VacancyDetails).HasMaxLength(500);
            entity.Property(e => e.VacancyHm)
                .HasMaxLength(50)
                .HasColumnName("VacancyHM");
            entity.Property(e => e.VacancyJobTitle).HasMaxLength(500);
            entity.Property(e => e.VacancyOd)
                .HasColumnType("date")
                .HasColumnName("VacancyOD");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
