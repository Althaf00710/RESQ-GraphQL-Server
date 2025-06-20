using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.Repositories.Generic;

namespace Core.Repositories.Interfaces
{
    public interface IEmergencyToVehicleRepository : IRepository<EmergencyToVehicle>
    {
        Task<bool> ExistsAsync(int emergencyCategoryId, int vehicleCategoryId);
        Task<IEnumerable<EmergencyToVehicle>> GetByEmergencyCategoryAsync(int emergencyCategoryId);
        Task<IEnumerable<EmergencyToVehicle>> GetByVehicleCategoryAsync(int vehicleCategoryId);
    }
}
