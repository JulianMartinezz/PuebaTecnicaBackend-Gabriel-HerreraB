using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

public partial class HRDbContext : DbContext
{
    public HRDbContext()
    {
    }

    public HRDbContext(DbContextOptions<HRDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<medical_record_type> medical_record_types { get; set; }

    public virtual DbSet<status> statuses { get; set; }

    public virtual DbSet<t_medical_record> t_medical_records { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=RRHH_DB;Username=postgres;Password=1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<medical_record_type>(entity =>
        {
            entity.HasKey(e => e.medical_record_type_id).HasName("medical_record_type_pkey");

            entity.ToTable("medical_record_type");

            entity.Property(e => e.description).HasMaxLength(500);
            entity.Property(e => e.name).HasMaxLength(100);
        });

        modelBuilder.Entity<status>(entity =>
        {
            entity.HasKey(e => e.status_id).HasName("status_pkey");

            entity.ToTable("status");

            entity.Property(e => e.description).HasMaxLength(500);
            entity.Property(e => e.name).HasMaxLength(100);
        });

        modelBuilder.Entity<t_medical_record>(entity =>
        {
            entity.HasKey(e => e.medical_record_id).HasName("t_medical_record_pkey");

            entity.ToTable("t_medical_record");

            entity.Property(e => e.area_change).HasMaxLength(2);
            entity.Property(e => e.audiometry).HasMaxLength(2);
            entity.Property(e => e.created_by).HasMaxLength(2000);
            entity.Property(e => e.deleted_by).HasMaxLength(2000);
            entity.Property(e => e.deletion_reason).HasMaxLength(2000);
            entity.Property(e => e.diagnosis).HasMaxLength(100);
            entity.Property(e => e.disability).HasMaxLength(2);
            entity.Property(e => e.disability_percentage).HasPrecision(10);
            entity.Property(e => e.execute_extra).HasMaxLength(2);
            entity.Property(e => e.execute_micros).HasMaxLength(2);
            entity.Property(e => e.father_data).HasMaxLength(2000);
            entity.Property(e => e.medical_board).HasMaxLength(200);
            entity.Property(e => e.modified_by).HasMaxLength(2000);
            entity.Property(e => e.mother_data).HasMaxLength(2000);
            entity.Property(e => e.observations).HasMaxLength(2000);
            entity.Property(e => e.other_family_data).HasMaxLength(2000);
            entity.Property(e => e.position_change).HasMaxLength(2);
            entity.Property(e => e.voice_evaluation).HasMaxLength(2);

            entity.HasOne(d => d.medical_record_type).WithMany(p => p.t_medical_records)
                .HasForeignKey(d => d.medical_record_type_id)
                .HasConstraintName("fk_medical_record_type");

            entity.HasOne(d => d.status).WithMany(p => p.t_medical_records)
                .HasForeignKey(d => d.status_id)
                .HasConstraintName("fk_status_id_record");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
