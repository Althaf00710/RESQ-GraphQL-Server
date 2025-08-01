using Core.Models;
using Core.Repositories.Generic;

namespace Core.Repositories.Interfaces
{
    public interface IRescueVehicleRepository : IRepository<RescueVehicle>
    {
        Task<string> GetMaxVehicleCodeAsync();
        IQueryable<RescueVehicle> Query();
        Task<RescueVehicle?> GetByPlateNumberAsync(string plateNumber);
    }
}
