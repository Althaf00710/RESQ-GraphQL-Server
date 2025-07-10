using Core.Models;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.RescueVehicleRequestApi
{
    [ExtendObjectType<Query>]
    public class RescueVehicleRequestQuery
    {
        public async Task<IEnumerable<RescueVehicleRequest>> GetRescueVehicleRequests([Service] IRescueVehicleRequestService service)
        {
            return await service.GetAllAsync();
        }

        public async Task<RescueVehicleRequest?> GetRescueVehicleRequestById(int id, [Service] IRescueVehicleRequestService service)
        {
            return await service.GetByIdAsync(id);
        }
    }
}
