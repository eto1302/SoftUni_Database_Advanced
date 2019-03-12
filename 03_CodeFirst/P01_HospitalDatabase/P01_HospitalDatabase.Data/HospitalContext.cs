using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;
using P01_HospitalDatabase.P01_HospitalDatabase.Data.EntityConfiguration;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext()
        {
            
        }

        public HospitalContext(DbContextOptions options) : base(options)
        {
            
        }

        private DbSet<Patient> Patients { get; set; }
        private DbSet<Visitation> Visitations { get; set; }
        private DbSet<Diagnose> Diagnoses { get; set; }
        private DbSet<Medicament> Medicaments { get; set; }
        private DbSet<PatientMedicament> Prescriptions { get; set; }
        private DbSet<Doctor> Doctors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=Hospital;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PatientConfig());
            modelBuilder.ApplyConfiguration(new VisitationConfig());
            modelBuilder.ApplyConfiguration(new DiagnoseConfig());
            modelBuilder.ApplyConfiguration(new MedicamentConfig());
            modelBuilder.ApplyConfiguration(new PatientMedicamentConfig());
            modelBuilder.ApplyConfiguration(new DoctorConfig());
        }
    }
}
