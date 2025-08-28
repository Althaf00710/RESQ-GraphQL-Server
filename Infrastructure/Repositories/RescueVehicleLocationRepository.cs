using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NetTopologySuite;

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

        public async Task<List<int>> GetNearestVehicleIdsAsync(double latitude, double longitude, int maxCount = 5)
        {
            var gf = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var origin = gf.CreatePoint(new Coordinate(longitude, latitude));

            maxCount = Math.Clamp(maxCount, 1, 5);

            // If you can guarantee one active row per vehicle, you can drop the GroupBy.
            return await _context.RescueVehicleLocations
                .AsNoTracking()
                .Where(l => l.Active && l.Location != null && l.RescueVehicle.Status == "Active")             // only current rows
                .GroupBy(l => l.RescueVehicleId)
                .Select(g => new
                {
                    RescueVehicleId = g.Key,
                    Distance = g.Min(l => l.Location.Distance(origin))  // meters for geography
                })
                .OrderBy(x => x.Distance)
                .Take(maxCount)
                .Select(x => x.RescueVehicleId)
                .ToListAsync();
        }
    }
}
