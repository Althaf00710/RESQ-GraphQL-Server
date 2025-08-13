using Core.Models;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.RescueVehicleLocationApi
{
    [ExtendObjectType<Query>]
    public class RescueVehicleLocationQuery
    {
        public async Task<IEnumerable<RescueVehicleLocation>> GetRescueVehicleLocations([Service] IRescueVehicleLocationService service)
        {
            return await service.GetAllAsync();
        }
    }
}
