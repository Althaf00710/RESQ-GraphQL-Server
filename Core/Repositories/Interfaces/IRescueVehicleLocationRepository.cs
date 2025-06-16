using Core.models;
using Core.Models;
using Core.Repositories.Generic;

namespace Core.Repositories.Interfaces
{
    public interface IRescueVehicleLocationRepository : IRepository<RescueVehicleLocation>
    {
        Task<bool> CheckRescueVehicleId(int rescueVehicleId);
        Task<RescueVehicleLocation> GetByRescueVehicleId(int rescueVehicleId);
    }
}
