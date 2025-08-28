using Core.DTO;
using Core.Models;

namespace RESQserver_dotnet.Api.RescueVehicleRequestApi
{
    [ExtendObjectType<Subscription>]
    public class RescueVehicleRequestSubscription
    {
        [Subscribe]
        [Topic("VehicleRequestStatusChanged")]
        public RescueVehicleRequest OnRescueVehicleRequestStatusChanged([EventMessage] RescueVehicleRequest request) => request;
    }
}
