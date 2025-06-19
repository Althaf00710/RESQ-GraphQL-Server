using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class FirstAidDetailRepository : Repository<FirstAidDetail>, IFirstAidDetailRepository
    {
        private readonly AppDbContext _context;

        public FirstAidDetailRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(int emergencySubCategoryId, string point, int? excludeId = null)
        {
            var query = _context.FirstAidDetails
                .Where(f => f.EmergencySubCategoryId == emergencySubCategoryId &&
                           f.Point == point);

            if (excludeId.HasValue)
            {
                query = query.Where(f => f.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<FirstAidDetail>> GetBySubCategoryIdAsync(int emergencySubCategoryId)
        {
            return await _context.FirstAidDetails
                .Where(f => f.EmergencySubCategoryId == emergencySubCategoryId)
                .OrderBy(f => f.DisplayOrder)
                .ToListAsync();
        }
    }
}
