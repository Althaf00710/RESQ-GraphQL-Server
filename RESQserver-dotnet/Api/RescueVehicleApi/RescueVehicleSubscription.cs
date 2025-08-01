using Core.Models;

namespace RESQserver_dotnet.Api.RescueVehicleApi
{
    [ExtendObjectType<Subscription>]
    public class RescueVehicleSubscription
    {
        [Subscribe]
        [Topic("VehicleStatusChanged")]
        public RescueVehicle OnVehicleStatusChanged([EventMessage] RescueVehicle vehicle) => vehicle;
    }
}
