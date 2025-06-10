using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CivilianStatusRepository : Repository<CivilianStatus>, ICivilianStatusRepository
    {
        private readonly AppDbContext _context;

        public CivilianStatusRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> RoleExistsAsync(string role, int? excludeId = null)
        {
            return await _context.CivilianStatuses.AnyAsync(cs =>
                cs.Role.ToLower() == role.ToLower() &&
                (!excludeId.HasValue || cs.Id != excludeId));
        }
    }
}
