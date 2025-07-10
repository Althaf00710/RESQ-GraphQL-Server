using Core.Models;
using Core.Repositories.Generic;

namespace Core.Repositories.Interfaces
{
    public interface IRescueVehicleRequestRepository : IRepository<RescueVehicleRequest>
    {
        Task<bool> CheckRecentReportings(double longitude, double latitude, int emergencyCategoryId);
    }
}
