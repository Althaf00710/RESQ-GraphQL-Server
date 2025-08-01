using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RescueVehicleRepository : Repository<RescueVehicle>, IRescueVehicleRepository
    {
        private readonly AppDbContext _context;
        public RescueVehicleRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RescueVehicle?> GetByPlateNumberAsync(string plateNumber)
        {
            return await _context.RescueVehicles
                .FirstOrDefaultAsync(v => v.PlateNumber == plateNumber);
        }

        public async Task<string> GetMaxVehicleCodeAsync()
        {
            return await _context.RescueVehicles
                .OrderByDescending(v => v.Code)
                .Select(v => v.Code)
                .FirstOrDefaultAsync();
        }

        public IQueryable<RescueVehicle> Query() =>
            _context.RescueVehicles.AsNoTracking();
    }
}
