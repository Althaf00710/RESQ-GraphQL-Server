using Core.Models;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.EmergencyToCivilianApi
{
    [ExtendObjectType<Query>]
    public class EmergencyToCivilianQuery
    {
        [UseFiltering]
        [UseSorting]
        public async Task<IEnumerable<EmergencyToCivilian>> GetEmergencyToCivilian([Service] IEmergencyToCivilianService emergencyToCivilianService)
        {
            return await emergencyToCivilianService.GetAllAsync();
        }

        public async Task<IEnumerable<EmergencyToCivilian>> GetMappingsByEmergencyCategory(int emergencyCategoryId, [Service] IEmergencyToCivilianService service)
        {
            return await service.GetByEmergencyCategoryAsync(emergencyCategoryId);
        }

        public async Task<IEnumerable<EmergencyToCivilian>> GetMappingsByCivilianStatus(int civilianStatusId, [Service] IEmergencyToCivilianService service)
        {
            return await service.GetByCivilianStatusAsync(civilianStatusId);
        }
    }
}
