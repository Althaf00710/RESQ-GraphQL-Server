using Core.Models;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.EmergencyToVehicleApi
{
    [ExtendObjectType<Query>]
    public class EmergencyToVehicleQuery
    {
        [UseFiltering]
        [UseSorting]
        public async Task<IEnumerable<EmergencyToVehicle>> GetEmergencyToVehicle([Service] IEmergencyToVehicleService emergencyToVehicleService)
        {
            return await emergencyToVehicleService.GetAllAsync();
        }

        public async Task<IEnumerable<EmergencyToVehicle>> GetVehicleByEmergencyCategory(int emergencyCategoryId, [Service] IEmergencyToVehicleService service)
        {
            return await service.GetByEmergencyCategoryAsync(emergencyCategoryId);
        }

        public async Task<IEnumerable<EmergencyToVehicle>> GetMappingsByVehicleCategory(int vehicleCategoryId, [Service] IEmergencyToVehicleService service)
        {
            return await service.GetByVehicleCategoryAsync(vehicleCategoryId);
        }
    }
}
