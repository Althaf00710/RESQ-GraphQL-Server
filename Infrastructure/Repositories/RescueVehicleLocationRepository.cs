using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RescueVehicleLocationRepository : Repository<RescueVehicleLocation>, IRescueVehicleLocationRepository
    {
        private readonly AppDbContext _context;
        public RescueVehicleLocationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> CheckRescueVehicleId(int rescueVehicleId)
        {
            return await Task.FromResult(_context.RescueVehicleLocations.Any(rvl => rvl.RescueVehicleId == rescueVehicleId));
        }

        public async  Task<RescueVehicleLocation> GetByRescueVehicleId(int rescueVehicleId)
        {
            return await _context.RescueVehicleLocations
                .FirstOrDefaultAsync(rvl => rvl.RescueVehicleId == rescueVehicleId);
        }
    }
}
