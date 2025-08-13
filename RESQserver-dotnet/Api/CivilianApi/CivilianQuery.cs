using Core.Models;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.CivilianApi
{
    [ExtendObjectType<Query>]
    public class CivilianQuery
    {
        [UsePaging(IncludeTotalCount = true)]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Civilian> GetCivilians([Service] ICivilianService service)
        {
            return service.Query();
        }

        public async Task<Civilian?> GetCivilianById(int id, [Service] ICivilianService service)
        {
            return await service.GetByIdAsync(id);
        }
    }
}
