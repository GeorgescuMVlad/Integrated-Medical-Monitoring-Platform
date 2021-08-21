using System;
using System.Collections.Generic;
using System.Text;
using IMMPlaH.Models;
using Microsoft.EntityFrameworkCore;

namespace IMMPlaH.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<User> User { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Caregiver> Caregivers { get; set; }

        public DbSet<Medication> Medications { get; set; }

        public DbSet<CaregiverPatients> CaregiverPatients { get; set; }

        public DbSet<Plan> Plans { get; set; }

        public DbSet<MedicationPlan> MedicationPlans { get; set; }

        public DbSet<Activities> Activities { get; set; }

        public DbSet<ActivitiesProblems> ActivitiesProblems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasOne(u => u.User)
                .WithOne(p => p.Patient)
                .HasForeignKey<Patient>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Caregiver>()
                .HasOne(u => u.User)
                .WithOne(c => c.Caregiver)
                .HasForeignKey<Caregiver>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CaregiverPatients>()
             .HasKey(c => new { c.CaregiverId, c.PatientId });

            modelBuilder.Entity<CaregiverPatients>()
                .HasOne(ca => ca.Caregiver)
                .WithMany(c => c.CaregiverPatients)
                .HasForeignKey(ca => ca.CaregiverId);

            modelBuilder.Entity<CaregiverPatients>()
                .HasOne(pa => pa.Patient)
                .WithMany(c => c.CaregiverPatients)
                .HasForeignKey(pa => pa.PatientId);

            modelBuilder.Entity<Plan>()
                .HasOne(p => p.Patient)
                .WithMany(pl => pl.Plans)
                .HasForeignKey(p => p.PatientId);

            modelBuilder.Entity<MedicationPlan>()
             .HasKey(p => new { p.PlanId, p.MedicationId });

            modelBuilder.Entity<MedicationPlan>()
                .HasOne(p => p.Plan)
                .WithMany(mp => mp.MedicationPlans)
                .HasForeignKey(p => p.PlanId);

            modelBuilder.Entity<MedicationPlan>()
                .HasOne(m => m.Medication)
                .WithMany(mp => mp.MedicationPlans)
                .HasForeignKey(m => m.MedicationId);

            modelBuilder.Entity<Activities>()
                .HasOne(a => a.Patient)
                .WithMany(ac => ac.Activities)
                .HasForeignKey(a => a.PatientId);

            modelBuilder.Entity<ActivitiesProblems>()
                .HasOne(ap => ap.Patient)
                .WithMany(acp => acp.ActivitiesProblems)
                .HasForeignKey(ap => ap.PatientId);


            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    Type = "doctor",
                    Email = "doctor1@doctor.com",
                    Password = "doctor1"
                },
                 new User()
                 {
                     Id = 2,
                     Type = "caregiver",
                     Email = "caregiver1@caregiver.com",
                     Password = "caregiver1"
                 },
                new User()
                {
                    Id = 3,
                    Type = "patient",
                    Email = "patient1@patient.com",
                    Password = "patient1"
                });

            modelBuilder.Entity<Patient>().HasData(
                new Patient()
                {
                    Id = 1,
                    Name = "Ion Popescu",
                    Birthday = "05/10/1964",
                    Gender = "Male",
                    Address = "Iasi, Str. Principala, nr. 2, bl. D3, sc. C, ap. 12",
                    MedicalRecord = "Diabet, insomnia and headaches",
                    UserId = 3
                });

            modelBuilder.Entity<Caregiver>().HasData(
                new Caregiver()
                {
                    Id = 1,
                    Name = "Adi Mutu",
                    Birthday = "10/10/1984",
                    Gender = "Male",
                    Address = "Bucuresti, Str. Victoriei, nr. 10, bl. A1, sc. A, ap. 10",
                    UserId = 2
                });

            modelBuilder.Entity<Medication>().HasData(
                new Medication()
                {
                    Id = 1,
                    Name = "Aspirina",
                    SideEffects = "Diareea, Headache, Stomach pain",
                    Dosage = 3
                });

        }
    }
}
