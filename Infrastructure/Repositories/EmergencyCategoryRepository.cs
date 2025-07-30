using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EmergencyCategoryRepository : Repository<EmergencyCategory>, IEmergencyCategoryRepository  
    {
        private readonly AppDbContext _context;

        public EmergencyCategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistAsync(string emergency, int? excludeId = null)
        {
            return await _context.EmergencyCategories.AnyAsync(ec =>
                ec.Name.ToLower() == emergency.ToLower() &&
                (!excludeId.HasValue || ec.Id != excludeId));
        }

        public async Task<List<EmergencyCategory>> UnmappedEmergencyToCivilian(int civilianStatusId)
        {
            return await _context.EmergencyCategories
                .Where(ec => !ec.EmergencyToCivilians
                    .Any(link => link.CivilianStatusId == civilianStatusId))
                .ToListAsync();
        }
    }
}
