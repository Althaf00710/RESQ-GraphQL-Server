using Core.Models;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.RescueVehicleTypeApi
{
    [ExtendObjectType<Query>]
    public class RescueVehicleCategoryQuery
    {
        public async Task<IEnumerable<RescueVehicleCategory>> GetRescueVehicleCategories([Service] IRescueVehicleCategoryService service)
        {
            return await service.GetAllAsync();
        }

        public async Task<RescueVehicleCategory?> GetRescueVehicleCategoryById(int id, [Service] IRescueVehicleCategoryService service)
        {
            return await service.GetByIdAsync(id);
        }

        public async Task<bool> IsRescueVehicleCategoryExists(string category, int? excludeId, [Service] IRescueVehicleCategoryService service)
        {
            return await service.CategoryExistsAsync(category, excludeId);
        }
    }
}
