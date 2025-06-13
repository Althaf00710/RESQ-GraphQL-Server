using Core.Models;
using Core.Repositories.Generic;

namespace Core.Repositories.Interfaces
{
    public interface IRescueVehicleCategoryRepository : IRepository<RescueVehicleCategory>
    {
        Task<bool> CategoryExistsAsync(string category, int? excludeId = null);
    }
}
