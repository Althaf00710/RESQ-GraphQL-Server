using Core.models;
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

        public async Task<string> GetMaxVehicleCodeAsync()
        {
            return await _context.RescueVehicles
                .OrderByDescending(v => v.Code)
                .Select(v => v.Code)
                .FirstOrDefaultAsync();
        }
    }
}
