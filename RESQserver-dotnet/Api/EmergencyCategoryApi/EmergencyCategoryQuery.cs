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

        public async Task<bool> GetEmergencyCategoryExist(string name, [Service] IEmergencyCategoryService service, int? excludeId = null)
        {
            return await service.CheckExist(name, excludeId);
        }

        public async Task<List<EmergencyCategory>> UnmappedEmergencyToCivilian(int civilianStatusId, [Service] IEmergencyCategoryService service)
        {
            return await service.UnmappedEmergencyToCivilian(civilianStatusId);
        }
    }
}
