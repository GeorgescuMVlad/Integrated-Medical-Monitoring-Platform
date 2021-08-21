﻿// <auto-generated />
using IMMPlaH.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IMMPlaH.DataAccess.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20201016115901_relationshipUser")]
    partial class relationshipUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IMMPlaH.Domain.Models.Caregiver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Birthday")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Caregivers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "Bucuresti, Str. Victoriei, nr. 10, bl. A1, sc. A, ap. 10",
                            Birthday = "10/10/1984",
                            Gender = "Male",
                            Name = "Adi Mutu",
                            UserId = 2
                        });
                });

            modelBuilder.Entity("IMMPlaH.Domain.Models.Medication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Dosage")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SideEffects")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Medications");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Dosage = 3,
                            Name = "Aspirina",
                            SideEffects = "Diareea, Headache, Stomach pain"
                        });
                });

            modelBuilder.Entity("IMMPlaH.Domain.Models.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Birthday")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MedicalRecord")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Patients");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "Iasi, Str. Principala, nr. 2, bl. D3, sc. C, ap. 12",
                            Birthday = "05/10/1964",
                            Gender = "Male",
                            MedicalRecord = "Diabet, insomnia and headaches",
                            Name = "Ion Popescu",
                            UserId = 3
                        });
                });

            modelBuilder.Entity("IMMPlaH.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "doctor1@doctor.com",
                            Password = "doctor1",
                            Type = "doctor"
                        },
                        new
                        {
                            Id = 2,
                            Email = "caregiver1@caregiver.com",
                            Password = "caregiver1",
                            Type = "caregiver"
                        },
                        new
                        {
                            Id = 3,
                            Email = "patient1@patient.com",
                            Password = "patient1",
                            Type = "patient"
                        });
                });

            modelBuilder.Entity("IMMPlaH.Domain.Models.Caregiver", b =>
                {
                    b.HasOne("IMMPlaH.Domain.Models.User", "User")
                        .WithOne("Caregiver")
                        .HasForeignKey("IMMPlaH.Domain.Models.Caregiver", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IMMPlaH.Domain.Models.Patient", b =>
                {
                    b.HasOne("IMMPlaH.Domain.Models.User", "User")
                        .WithOne("Patient")
                        .HasForeignKey("IMMPlaH.Domain.Models.Patient", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
