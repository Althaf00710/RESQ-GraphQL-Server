using Core.Models;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.CivilianLocationApi
{
    [ExtendObjectType<Query>]
    public class CivilianLocationQuery
    {
        public async Task<IEnumerable<CivilianLocation>> GetCivilianLocations([Service] ICivilianLocationService service)
        {
            return await service.GetAllAsync();
        }

        public async Task<CivilianLocation?> GetCivilianLocationById(int id, [Service] ICivilianLocationService service)
        {
            return await service.GetByCivilianId(id);
        }
    }
}
