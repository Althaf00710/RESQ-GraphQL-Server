using Core.Models;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.EmergencyCategoryApi
{
    [ExtendObjectType<Query>]
    public class EmergencyCategoryQuery
    {
        public async Task<IEnumerable<EmergencyCategory>> GetEmergencyCategories([Service] IEmergencyCategoryService service)
        {
            return await service.GetAllAsync();
        }

        public async Task<EmergencyCategory?> GetEmergencyCategoryById(int id, [Service] IEmergencyCategoryService service)
        {
            return await service.GetByIdAsync(id);
        }
    }
}
