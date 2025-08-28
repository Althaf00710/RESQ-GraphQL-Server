using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Infrastructure.Repositories
{
    public class RescueVehicleRequestRepository : Repository<RescueVehicleRequest>, IRescueVehicleRequestRepository
    {
        private readonly AppDbContext _context;
        private readonly GeometryFactory _geometryFactory;
        private const double SearchRadiusMeters = 50;
        private const int TimeGapMinutes = 10; // In Sri Lanka Average response time is 11 - 15 minutes

        public RescueVehicleRequestRepository(AppDbContext context, GeometryFactory geometryFactory) : base(context)
        {
            _context = context;
            _geometryFactory = geometryFactory;
        }

        public async Task<bool> CheckRecentReportings(double longitude, double latitude, int emergencySubCategoryId)
        {
            try
            {
                var timeThreshold = DateTime.UtcNow.AddMinutes(-TimeGapMinutes);
                var searchPoint = _geometryFactory.CreatePoint(new Coordinate(longitude, latitude));

                // Distance is in meters when the column is 'geography'
                var exists = await _context.RescueVehicleRequests
                    .Where(r => r.EmergencySubCategoryId == emergencySubCategoryId && r.CreatedAt >= timeThreshold)
                    .Where(r => r.Location != null && r.Location.Distance(searchPoint) <= SearchRadiusMeters)
                    .AnyAsync();

                return exists;
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking recent reportings", ex);
            }
        }

        public IQueryable<RescueVehicleRequest> Query() =>
            _context.RescueVehicleRequests.AsNoTracking();
    }
}
