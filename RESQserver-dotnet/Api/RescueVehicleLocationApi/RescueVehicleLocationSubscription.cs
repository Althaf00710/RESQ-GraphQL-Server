using Core.Models;

namespace RESQserver_dotnet.Api.RescueVehicleLocationApi
{
    [ExtendObjectType<Subscription>]
    public class RescueVehicleLocationSubscription
    {
        [Subscribe]
        [Topic("VehicleLocationShare")]
        public RescueVehicleLocation OnVehicleLocationShare([EventMessage] RescueVehicleLocation vehicleLocation) => vehicleLocation;
    }
}
