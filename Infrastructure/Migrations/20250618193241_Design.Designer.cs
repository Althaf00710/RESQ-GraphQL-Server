﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250618193241_Design")]
    partial class Design
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Core.Models.Civilian", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CivilianStatusId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("JoinedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NicNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CivilianStatusId");

                    b.ToTable("Civilians");
                });

            modelBuilder.Entity("Core.Models.CivilianLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("CivilianId")
                        .HasColumnType("int");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CivilianId");

                    b.ToTable("CivilianLocations");
                });

            modelBuilder.Entity("Core.Models.CivilianStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CivilianStatuses");
                });

            modelBuilder.Entity("Core.Models.CivilianStatusRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CivilianId")
                        .HasColumnType("int");

                    b.Property<int>("CivilianStatusId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("proofImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CivilianId");

                    b.HasIndex("CivilianStatusId");

                    b.ToTable("CivilianStatusRequests");
                });

            modelBuilder.Entity("Core.Models.EmergencyCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Icon")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EmergencyCategories");
                });

            modelBuilder.Entity("Core.Models.EmergencySubCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EmergencyCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EmergencyCategoryId");

                    b.ToTable("EmergencySubCategories");
                });

            modelBuilder.Entity("Core.Models.EmergencyToCivilian", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CivilianStatusId")
                        .HasColumnType("int");

                    b.Property<int>("EmergencyCategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CivilianStatusId");

                    b.HasIndex("EmergencyCategoryId");

                    b.ToTable("EmergencyToCivilians");
                });

            modelBuilder.Entity("Core.Models.EmergencyToVehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EmergencyCategoryId")
                        .HasColumnType("int");

                    b.Property<int>("VehicleCategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmergencyCategoryId");

                    b.HasIndex("VehicleCategoryId");

                    b.ToTable("EmergencyToVehicles");
                });

            modelBuilder.Entity("Core.Models.FirstAidDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<int>("EmergencySubCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Point")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EmergencySubCategoryId");

                    b.ToTable("FirstAidDetails");
                });

            modelBuilder.Entity("Core.Models.RescueVehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlateNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RescueVehicleCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RescueVehicleCategoryId");

                    b.ToTable("RescueVehicles");
                });

            modelBuilder.Entity("Core.Models.RescueVehicleAssignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("RescueVehicleId")
                        .HasColumnType("int");

                    b.Property<int>("RescueVehicleRequestId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("RescueVehicleId");

                    b.HasIndex("RescueVehicleRequestId");

                    b.ToTable("RescueVehicleAssignments");
                });

            modelBuilder.Entity("Core.Models.RescueVehicleCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RescueVehicleCategories");
                });

            modelBuilder.Entity("Core.Models.RescueVehicleLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<int>("RescueVehicleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RescueVehicleId");

                    b.ToTable("RescueVehicleLocations");
                });

            modelBuilder.Entity("Core.Models.RescueVehicleRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CivilianId")
                        .HasColumnType("int");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("proofImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CivilianId");

                    b.ToTable("RescueVehicleRequests");
                });

            modelBuilder.Entity("Core.Models.SnakeType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ScientificName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VenomType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SnakeTypes");
                });

            modelBuilder.Entity("Core.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("JoinedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastActive")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicturePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Core.Models.Civilian", b =>
                {
                    b.HasOne("Core.Models.CivilianStatus", "CivilianStatus")
                        .WithMany("Civilians")
                        .HasForeignKey("CivilianStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CivilianStatus");
                });

            modelBuilder.Entity("Core.Models.CivilianLocation", b =>
                {
                    b.HasOne("Core.Models.Civilian", "Civilian")
                        .WithMany("CivilianLocations")
                        .HasForeignKey("CivilianId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Civilian");
                });

            modelBuilder.Entity("Core.Models.CivilianStatusRequest", b =>
                {
                    b.HasOne("Core.Models.Civilian", "Civilian")
                        .WithMany("CivilianStatusRequests")
                        .HasForeignKey("CivilianId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Core.Models.CivilianStatus", "CivilianStatus")
                        .WithMany("CivilianStatusRequests")
                        .HasForeignKey("CivilianStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Civilian");

                    b.Navigation("CivilianStatus");
                });

            modelBuilder.Entity("Core.Models.EmergencySubCategory", b =>
                {
                    b.HasOne("Core.Models.EmergencyCategory", "EmergencyCategory")
                        .WithMany()
                        .HasForeignKey("EmergencyCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EmergencyCategory");
                });

            modelBuilder.Entity("Core.Models.EmergencyToCivilian", b =>
                {
                    b.HasOne("Core.Models.CivilianStatus", "CivilianStatus")
                        .WithMany("EmergencyToCivilians")
                        .HasForeignKey("CivilianStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Models.EmergencyCategory", "EmergencyCategory")
                        .WithMany("EmergencyToCivilians")
                        .HasForeignKey("EmergencyCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CivilianStatus");

                    b.Navigation("EmergencyCategory");
                });

            modelBuilder.Entity("Core.Models.EmergencyToVehicle", b =>
                {
                    b.HasOne("Core.Models.EmergencyCategory", "EmergencyCategory")
                        .WithMany("EmergencyToVehicles")
                        .HasForeignKey("EmergencyCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Models.RescueVehicleCategory", "RescueVehicleCategory")
                        .WithMany("EmergencyToVehicles")
                        .HasForeignKey("VehicleCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EmergencyCategory");

                    b.Navigation("RescueVehicleCategory");
                });

            modelBuilder.Entity("Core.Models.FirstAidDetail", b =>
                {
                    b.HasOne("Core.Models.EmergencySubCategory", "EmergencySubCategory")
                        .WithMany("FirstAidDetails")
                        .HasForeignKey("EmergencySubCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EmergencySubCategory");
                });

            modelBuilder.Entity("Core.Models.RescueVehicle", b =>
                {
                    b.HasOne("Core.Models.RescueVehicleCategory", "RescueVehicleCategory")
                        .WithMany("RescueVehicles")
                        .HasForeignKey("RescueVehicleCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RescueVehicleCategory");
                });

            modelBuilder.Entity("Core.Models.RescueVehicleAssignment", b =>
                {
                    b.HasOne("Core.Models.RescueVehicle", "RescueVehicle")
                        .WithMany("RescueVehicleAssignment")
                        .HasForeignKey("RescueVehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Models.RescueVehicleRequest", "RescueVehicleRequest")
                        .WithMany("RescueVehicleAssignments")
                        .HasForeignKey("RescueVehicleRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RescueVehicle");

                    b.Navigation("RescueVehicleRequest");
                });

            modelBuilder.Entity("Core.Models.RescueVehicleLocation", b =>
                {
                    b.HasOne("Core.Models.RescueVehicle", "RescueVehicle")
                        .WithMany("RescueVehicleLocations")
                        .HasForeignKey("RescueVehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RescueVehicle");
                });

            modelBuilder.Entity("Core.Models.RescueVehicleRequest", b =>
                {
                    b.HasOne("Core.Models.Civilian", "Civilian")
                        .WithMany("RescueVehicleRequests")
                        .HasForeignKey("CivilianId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Civilian");
                });

            modelBuilder.Entity("Core.Models.Civilian", b =>
                {
                    b.Navigation("CivilianLocations");

                    b.Navigation("CivilianStatusRequests");

                    b.Navigation("RescueVehicleRequests");
                });

            modelBuilder.Entity("Core.Models.CivilianStatus", b =>
                {
                    b.Navigation("CivilianStatusRequests");

                    b.Navigation("Civilians");

                    b.Navigation("EmergencyToCivilians");
                });

            modelBuilder.Entity("Core.Models.EmergencyCategory", b =>
                {
                    b.Navigation("EmergencyToCivilians");

                    b.Navigation("EmergencyToVehicles");
                });

            modelBuilder.Entity("Core.Models.EmergencySubCategory", b =>
                {
                    b.Navigation("FirstAidDetails");
                });

            modelBuilder.Entity("Core.Models.RescueVehicle", b =>
                {
                    b.Navigation("RescueVehicleAssignment");

                    b.Navigation("RescueVehicleLocations");
                });

            modelBuilder.Entity("Core.Models.RescueVehicleCategory", b =>
                {
                    b.Navigation("EmergencyToVehicles");

                    b.Navigation("RescueVehicles");
                });

            modelBuilder.Entity("Core.Models.RescueVehicleRequest", b =>
                {
                    b.Navigation("RescueVehicleAssignments");
                });
#pragma warning restore 612, 618
        }
    }
}
