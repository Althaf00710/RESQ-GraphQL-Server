using Core.Models;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.CivilianStatusRequestApi
{
    [ExtendObjectType<Query>]
    public class CivilianStatusRequestQuery
    {
        public async Task<IEnumerable<CivilianStatusRequest>> GetCivilianStatusRequests([Service] ICivilianStatusRequestService service)
        {
            return await service.GetAllAsync();
        }
        public async Task<CivilianStatusRequest?> GetCivilianStatusRequestById(int id, [Service] ICivilianStatusRequestService service)
        {
            return await service.GetByIdAsync(id);
        }
    }
}
