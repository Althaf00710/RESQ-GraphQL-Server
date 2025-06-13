using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RescueVehicleCategoryRepository : Repository<RescueVehicleCategory>, IRescueVehicleCategoryRepository
    {
        private readonly AppDbContext _context;

        public RescueVehicleCategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> CategoryExistsAsync(string category, int? excludeId = null)
        {
            return await _context.RescueVehicleCategories.AnyAsync(rvc =>
                rvc.Name.ToLower() == category.ToLower() &&
                (!excludeId.HasValue || rvc.Id != excludeId));
        }
    }
}






