using Core.Models;
using Core.Repositories.Generic;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CivilianLocationRepository : Repository<CivilianLocation>, ICivilianLocationRepository
    {
        private readonly AppDbContext _context;

        public CivilianLocationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> CheckCivilianId(int civilianId)
        {
            return await Task.FromResult(_context.CivilianLocations.Any(cl => cl.CivilianId == civilianId));
        }

        public async Task<CivilianLocation> GetByCivilianId(int civilianId)
        {
            return await _context.CivilianLocations.FirstOrDefaultAsync(cl => cl.CivilianId == civilianId);
        }
    }
}
