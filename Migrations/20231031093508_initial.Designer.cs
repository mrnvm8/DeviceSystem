﻿// <auto-generated />
using System;
using DeviceSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DeviceSystem.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20231031093508_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.24")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DeviceSystem.Models.Department", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("OfficeId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("OfficeId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("DeviceSystem.Models.Device", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Condition")
                        .HasColumnType("int");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("char(36)");

                    b.Property<string>("DeviceIMEINo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("DeviceName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("DeviceSerialNo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("DeviceTypeId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("PurchasedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("PurchasedPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("DeviceTypeId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("DeviceSystem.Models.DeviceLoans", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("AssignedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("DeviceId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("ReturnDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("ReturnToUserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("UserId");

                    b.ToTable("DeviceLoans");
                });

            modelBuilder.Entity("DeviceSystem.Models.DeviceSummary", b =>
                {
                    b.Property<int>("Assigned")
                        .HasColumnType("int");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("char(36)");

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("DeviceTypeId")
                        .HasColumnType("char(36)");

                    b.Property<string>("OfficeName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Total")
                        .HasColumnType("int");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Unavailable")
                        .HasColumnType("int");

                    b.ToTable("DevicesSummaries");
                });

            modelBuilder.Entity("DeviceSystem.Models.DeviceType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("DeviceTypes");
                });

            modelBuilder.Entity("DeviceSystem.Models.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Enrollment")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsEmployeeActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("char(36)");

                    b.Property<string>("WorkEmail")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("PersonId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("DeviceSystem.Models.Office", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Offices");
                });

            modelBuilder.Entity("DeviceSystem.Models.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("People");
                });

            modelBuilder.Entity("DeviceSystem.Models.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("ArangedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("DeviceId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("FixedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IssueSolved")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("TicketCreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("TicketIssue")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("TicketSolution")
                        .HasColumnType("longtext");

                    b.Property<string>("TicketTitle")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("TicketUpdate")
                        .HasColumnType("longtext");

                    b.Property<bool>("Updated")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("UserId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("DeviceSystem.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("char(36)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DeviceSystem.Models.Department", b =>
                {
                    b.HasOne("DeviceSystem.Models.Office", "Offices")
                        .WithMany("Departments")
                        .HasForeignKey("OfficeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Offices");
                });

            modelBuilder.Entity("DeviceSystem.Models.Device", b =>
                {
                    b.HasOne("DeviceSystem.Models.Department", "Department")
                        .WithMany("Devices")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeviceSystem.Models.DeviceType", "DeviceType")
                        .WithMany("Devices")
                        .HasForeignKey("DeviceTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("DeviceType");
                });

            modelBuilder.Entity("DeviceSystem.Models.DeviceLoans", b =>
                {
                    b.HasOne("DeviceSystem.Models.Device", "Device")
                        .WithMany("DevicesLoans")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeviceSystem.Models.Employee", "Employee")
                        .WithMany("DevicesLoans")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeviceSystem.Models.User", "User")
                        .WithMany("DevicesLoans")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("Employee");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DeviceSystem.Models.Employee", b =>
                {
                    b.HasOne("DeviceSystem.Models.Department", "Department")
                        .WithMany("Employees")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeviceSystem.Models.Person", "Person")
                        .WithMany("Employees")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("DeviceSystem.Models.Ticket", b =>
                {
                    b.HasOne("DeviceSystem.Models.Device", "Devices")
                        .WithMany("Tickets")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeviceSystem.Models.User", "Users")
                        .WithMany("Tickets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Devices");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("DeviceSystem.Models.User", b =>
                {
                    b.HasOne("DeviceSystem.Models.Employee", "Employee")
                        .WithMany("Users")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("DeviceSystem.Models.Department", b =>
                {
                    b.Navigation("Devices");

                    b.Navigation("Employees");
                });

            modelBuilder.Entity("DeviceSystem.Models.Device", b =>
                {
                    b.Navigation("DevicesLoans");

                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("DeviceSystem.Models.DeviceType", b =>
                {
                    b.Navigation("Devices");
                });

            modelBuilder.Entity("DeviceSystem.Models.Employee", b =>
                {
                    b.Navigation("DevicesLoans");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("DeviceSystem.Models.Office", b =>
                {
                    b.Navigation("Departments");
                });

            modelBuilder.Entity("DeviceSystem.Models.Person", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("DeviceSystem.Models.User", b =>
                {
                    b.Navigation("DevicesLoans");

                    b.Navigation("Tickets");
                });
#pragma warning restore 612, 618
        }
    }
}