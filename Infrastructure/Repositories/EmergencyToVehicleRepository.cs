using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EmergencyToVehicleRepository : Repository<EmergencyToVehicle>, IEmergencyToVehicleRepository
    {
        private readonly AppDbContext _context;

        public EmergencyToVehicleRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(int emergencyCategoryId, int vehicleCategoryId)
        {
            return await _context.EmergencyToVehicles
                .AnyAsync(etv => etv.EmergencyCategoryId == emergencyCategoryId &&
                               etv.VehicleCategoryId == vehicleCategoryId);
        }

        public async Task<IEnumerable<EmergencyToVehicle>> GetByEmergencyCategoryAsync(int emergencyCategoryId)
        {
            return await _context.EmergencyToVehicles
                .Where(etv => etv.EmergencyCategoryId == emergencyCategoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmergencyToVehicle>> GetByVehicleCategoryAsync(int vehicleCategoryId)
        {
            return await _context.EmergencyToVehicles
                .Where(etv => etv.VehicleCategoryId == vehicleCategoryId)
                .ToListAsync();
        }
    }
}
