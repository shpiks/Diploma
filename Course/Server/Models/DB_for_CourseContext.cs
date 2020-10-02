using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Server.Models
{
    public partial class DB_for_CourseContext : DbContext
    {
        public DB_for_CourseContext()
        {
            //Database.EnsureCreated();
        }

        public DB_for_CourseContext(DbContextOptions<DB_for_CourseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<MaterialEmployees> MaterialEmployees { get; set; }
        public virtual DbSet<Materials> Materials { get; set; }
        public virtual DbSet<VictimMaterials> VictimMaterials { get; set; }
        public virtual DbSet<Victims> Victims { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //                optionsBuilder.UseSqlite("Data Source=.\\App_Data\\DB_for_Course.db;");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);

                entity.HasIndex(e => e.EmployeeId)
                    .IsUnique();

                //entity.Property(e => e.EmployeeId).ValueGeneratedNever();
                entity.Property(e => e.EmployeeId).ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.LastName).IsRequired();

                entity.Property(e => e.Position).IsRequired();
            });

            modelBuilder.Entity<MaterialEmployees>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeEmployeeId, e.MaterialMaterialId });

                entity.Property(e => e.EmployeeEmployeeId).HasColumnName("Employee_EmployeeId");

                entity.Property(e => e.MaterialMaterialId).HasColumnName("Material_MaterialId");

                entity.HasOne(d => d.EmployeeEmployee)
                    .WithMany(p => p.MaterialEmployees)
                    .HasForeignKey(d => d.EmployeeEmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.MaterialMaterial)
                    .WithMany(p => p.MaterialEmployees)
                    .HasForeignKey(d => d.MaterialMaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Materials>(entity =>
            {
                entity.HasKey(e => e.MaterialId);

                entity.HasIndex(e => e.MaterialId)
                    .IsUnique();

                //entity.Property(e => e.MaterialId).ValueGeneratedNever();
                entity.Property(e => e.MaterialId).ValueGeneratedOnAdd();
                

                entity.Property(e => e.DateOfRegistration).IsRequired();

                entity.Property(e => e.DateOfTerm).IsRequired();

                entity.Property(e => e.NumberEk).HasColumnName("NumberEK");
            });

            modelBuilder.Entity<VictimMaterials>(entity =>
            {
                entity.HasKey(e => new { e.MaterialMaterialId, e.VictimVictimId });

                entity.Property(e => e.MaterialMaterialId).HasColumnName("Material_MaterialId");

                entity.Property(e => e.VictimVictimId).HasColumnName("Victim_VictimId");

                entity.HasOne(d => d.MaterialMaterial)
                    .WithMany(p => p.VictimMaterials)
                    .HasForeignKey(d => d.MaterialMaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.VictimVictim)
                    .WithMany(p => p.VictimMaterials)
                    .HasForeignKey(d => d.VictimVictimId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Victims>(entity =>
            {
                entity.HasKey(e => e.VictimId);

                entity.HasIndex(e => e.VictimId)
                    .IsUnique();

                //entity.Property(e => e.VictimId).ValueGeneratedNever();
                entity.Property(e => e.VictimId).ValueGeneratedOnAdd();

                entity.Property(e => e.DateOfBirth).IsRequired();

                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.LastName).IsRequired();

                entity.Property(e => e.Patronymic).IsRequired();
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.HasIndex(e => e.UserId)
                    .IsUnique();

                //entity.Property(e => e.VictimId).ValueGeneratedNever();
                entity.Property(e => e.UserId).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.Role).IsRequired();

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
