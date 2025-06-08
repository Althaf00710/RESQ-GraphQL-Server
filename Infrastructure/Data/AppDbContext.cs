using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.models;
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

            modelBuilder.Entity<CivilianTypeRequest>()
                .HasOne(r => r.Civilian)
                .WithMany(c => c.CivilianTypeRequests)
                .HasForeignKey(r => r.CivilianId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CivilianTypeRequest>()
                .HasOne(r => r.CivilianType)
                .WithMany(t => t.CivilianTypeRequests)
                .HasForeignKey(r => r.CivilianTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        }


        public DbSet<Civilian> Civilians { get; set; }
        public DbSet<CivilianLocation> CivilianLocations { get; set; }
        public DbSet<CivilianType> CivilianTypes { get; set; }
        public DbSet<CivilianTypeRequest> CivilianTypeRequests { get; set; }

        public DbSet<FirstAidCategory> FirstAidCategories { get; set; }
        public DbSet<FirstAidDetail> FirstAidDetails { get; set; }
        public DbSet<FirstAid> FirstAids { get; set; }

        public DbSet<RescueVehicle> RescueVehicles { get; set; }
        public DbSet<RescueVehicleLocation> RescueVehicleLocations { get; set; }
        public DbSet<RescueVehicleType> RescueVehicleTypes { get; set; }
        public DbSet<RescueVehicleRequest> RescueVehicleRequests { get; set; }
        public DbSet<RescueVehicleAssignment> RescueVehicleAssignments { get; set; }

        public DbSet<SnakeType> SnakeTypes { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
