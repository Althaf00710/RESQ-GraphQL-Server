using Core.Models;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.CivilianStatusRequestApi
{
    [ExtendObjectType<Query>]
    public class CivilianStatusRequestQuery
    {
        [UsePaging(IncludeTotalCount = true)]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<CivilianStatusRequest> GetCivilianStatusRequests([Service] ICivilianStatusRequestService service)
        {
            return service.Query();
        }

        public async Task<CivilianStatusRequest?> GetCivilianStatusRequestById(int id, [Service] ICivilianStatusRequestService service)
        {
            return await service.GetByIdAsync(id);
        }

    }
}
