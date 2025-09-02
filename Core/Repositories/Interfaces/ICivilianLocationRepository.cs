using Core.Models;
using Core.Repositories.Generic;

namespace Core.Repositories.Interfaces
{
    public interface ICivilianLocationRepository : IRepository<CivilianLocation>
    {
        Task<bool> CheckCivilianId(int civilianId);
        Task<CivilianLocation> GetByCivilianId(int civilianId);
        Task<List<int>> GetNearbyCivilianIdsAsync(
            double latitude,
            double longitude,
            int emergencyCategoryId,
            double radiusMeters = 100);
    }
}
