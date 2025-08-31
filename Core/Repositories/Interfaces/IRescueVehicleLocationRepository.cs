
using Core.Models;
using Core.Repositories.Generic;

namespace Core.Repositories.Interfaces
{
    public interface IRescueVehicleLocationRepository : IRepository<RescueVehicleLocation>
    {
        Task<bool> CheckRescueVehicleId(int rescueVehicleId);
        Task<RescueVehicleLocation> GetByRescueVehicleId(int rescueVehicleId);
        Task<List<int>> GetNearestVehicleIdsAsync(double latitude, double longitude,int subCategoryId, int maxCount = 5);
    }
}
