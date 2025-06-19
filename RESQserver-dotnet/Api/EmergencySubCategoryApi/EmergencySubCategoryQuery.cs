using Core.Models;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.EmergencySubCategoryApi
{
    [ExtendObjectType<Query>]
    public class EmergencySubCategoryQuery
    {
        public async Task<IEnumerable<EmergencySubCategory>> GetEmergencySubCategories([Service] IEmergencySubCategoryService service)
        {
            return await service.GetAllAsync();
        }

        public async Task<EmergencySubCategory?> GetEmergencySubCategoryById(int id, [Service] IEmergencySubCategoryService service)
        {
            return await service.GetByIdAsync(id);
        }
    }
}
