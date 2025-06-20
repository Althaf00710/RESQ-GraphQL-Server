using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO;
using Core.Models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface IEmergencyToVehicleService : IService<EmergencyToVehicle>
    {
        Task<EmergencyToVehicle> Add(EmergencyToVehicleInput input);
        Task<bool> Delete(int id);
        Task<IEnumerable<EmergencyToVehicle>> GetByEmergencyCategoryAsync(int emergencyCategoryId);
        Task<IEnumerable<EmergencyToVehicle>> GetByVehicleCategoryAsync(int vehicleCategoryId);
    }
}
