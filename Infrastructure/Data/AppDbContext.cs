using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CivilianStatusRequest>()
                .HasOne(r => r.Civilian)
                .WithMany(c => c.CivilianStatusRequests)
                .HasForeignKey(r => r.CivilianId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CivilianStatusRequest>()
                .HasOne(r => r.CivilianStatus)
                .WithMany(t => t.CivilianStatusRequests)
                .HasForeignKey(r => r.CivilianStatusId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                x => x.UseNetTopologySuite()
            );
        }

        public DbSet<Civilian> Civilians { get; set; }
        public DbSet<CivilianLocation> CivilianLocations { get; set; }
        public DbSet<CivilianStatus> CivilianStatuses { get; set; }
        public DbSet<CivilianStatusRequest> CivilianStatusRequests { get; set; }

        public DbSet<FirstAidDetail> FirstAidDetails { get; set; }

        public DbSet<RescueVehicle> RescueVehicles { get; set; }
        public DbSet<RescueVehicleLocation> RescueVehicleLocations { get; set; }
        public DbSet<RescueVehicleCategory> RescueVehicleCategories { get; set; }
        public DbSet<RescueVehicleRequest> RescueVehicleRequests { get; set; }
        public DbSet<RescueVehicleAssignment> RescueVehicleAssignments { get; set; }

        public DbSet<EmergencyCategory> EmergencyCategories { get; set; }
        public DbSet<EmergencySubCategory> EmergencySubCategories { get; set; }
        public DbSet<EmergencyToCivilian> EmergencyToCivilians { get; set; }
        public DbSet<EmergencyToVehicle> EmergencyToVehicles { get; set; }

        public DbSet<Snake> Snakes { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
