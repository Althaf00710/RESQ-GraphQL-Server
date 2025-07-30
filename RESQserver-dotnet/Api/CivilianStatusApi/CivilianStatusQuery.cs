using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.CivilianStatusApi
{
    [ExtendObjectType<Query>]
    public class CivilianStatusQuery
    {
        public async Task<IEnumerable<CivilianStatus>> GetCivilianStatuses([Service] ICivilianStatusService service)
        {
            return await service.GetAllAsync();
        }

        public async Task<CivilianStatus?> GetCivilianStatusById(int id, [Service] ICivilianStatusService service)
        {
            return await service.GetByIdAsync(id);
        }

        public async Task<bool> IsCivilianStatusRoleExists(string role, int? excludeId, [Service] ICivilianStatusService service)
        {
            return await service.RoleExistAsync(role, excludeId);
        }
    }
}
