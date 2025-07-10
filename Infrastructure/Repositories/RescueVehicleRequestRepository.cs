using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RescueVehicleRequestRepository : Repository<RescueVehicleRequest>, IRescueVehicleRequestRepository
    {
        private readonly AppDbContext _context;
        private const double SearchRadius = 50;
        private const int TimeGap = 10; // In Sri Lanka Average response time is 11 - 15 minutes

        public RescueVehicleRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> CheckRecentReportings(double longitude, double latitude, int emergencyCategoryId)
        {
            try
            {
                var now = DateTime.UtcNow;
                var timeThreshold = now.AddMinutes(-TimeGap);

                // Using Haversine formula 
                // with PostGIS Better but ippothaiku ithu ok
                var nearbyRequests = await _context.RescueVehicleRequests
                    .Where(r => r.EmergencyCategoryId == emergencyCategoryId &&
                                r.CreatedAt >= timeThreshold &&
                                (Math.Pow(69.1 * (r.Latitude - latitude), 2) +
                                 Math.Pow(69.1 * (longitude - r.Longitude) * Math.Cos(latitude * Math.PI / 180), 2)) * 1609.344 <= SearchRadius)
                    .AnyAsync();

                return nearbyRequests;
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking recent reportings", ex);
            }
        }
    }
}
