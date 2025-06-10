using Core.Models;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.CivilianApi
{
    [ExtendObjectType<Query>]
    public class CivilianQuery
    {
        public async Task<IEnumerable<Civilian>> GetCivilians([Service] ICivilianService service)
        {
            return await service.GetAllAsync();
        }

        public async Task<Civilian?> GetCivilianById(int id, [Service] ICivilianService service)
        {
            return await service.GetByIdAsync(id);
        }
    }
}
