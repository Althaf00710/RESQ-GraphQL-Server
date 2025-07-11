using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RescueVehicleAssignmentRepository : Repository<RescueVehicleAssignment>, IRescueVehicleAssignmentRepository
    {
        private readonly AppDbContext _context;
        public RescueVehicleAssignmentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RescueVehicleAssignment?> GetActiveAssignmentAsync(int vehicleId)
        {
            return await _context.RescueVehicleAssignments
                .Include(a => a.RescueVehicleRequest)
                .Include(a => a.RescueVehicle)
                .FirstOrDefaultAsync(a =>
                    a.RescueVehicleId == vehicleId &&
                    a.Status != "Completed" &&
                    a.Status != "Cancelled");
        }
    }
}
