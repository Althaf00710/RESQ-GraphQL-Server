using Core.Models;
using Core.Repositories.Generic;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NetTopologySuite;

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

        public async Task<List<int>> GetNearbyCivilianIdsAsync(
            double latitude,
            double longitude,
            int emergencyCategoryId,
            double radiusMeters = 100)
        {
            var gf = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var origin = gf.CreatePoint(new Coordinate(longitude, latitude));

            // Which civilian statuses are eligible for this emergency category
            var allowedStatusIds = await _context.EmergencyToCivilians
                .AsNoTracking()
                .Where(m => m.EmergencyCategoryId == emergencyCategoryId)
                .Select(m => m.CivilianStatusId)
                .Distinct()
                .ToListAsync();

            if (allowedStatusIds.Count == 0)
                return new List<int>();

            return await _context.CivilianLocations
                .AsNoTracking()
                .Where(cl =>
                    cl.Active &&
                    cl.Location != null &&
                    allowedStatusIds.Contains(cl.Civilian.CivilianStatusId)
                )
                .GroupBy(cl => cl.CivilianId)
                .Select(g => new
                {
                    CivilianId = g.Key,
                    Distance = g.Min(cl => cl.Location.Distance(origin)) // STDistance (meters) on geography
                })
                .Where(x => x.Distance <= radiusMeters)
                .OrderBy(x => x.Distance)
                .Select(x => x.CivilianId)
                .ToListAsync();
        }
    }
}
