using Core.Models;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;

namespace RESQserver_dotnet.Api.RescueVehicleLocationApi
{
    [ExtendObjectType<Subscription>]
    public class RescueVehicleLocationSubscription
    {
        [Subscribe]
        [Topic("VehicleLocationShare")]
        public RescueVehicleLocation OnVehicleLocationShare([EventMessage] RescueVehicleLocation vehicleLocation) => vehicleLocation;

        [Subscribe(With = nameof(SubscribeToVehicleAsync))]
        public RescueVehicleLocation OnVehicleLocationShareByVehicle(
            int rescueVehicleId,
            [EventMessage] RescueVehicleLocation vehicleLocation) => vehicleLocation;

        public ValueTask<ISourceStream<RescueVehicleLocation>> SubscribeToVehicleAsync(
            int rescueVehicleId,
            [Service] ITopicEventReceiver receiver,
            CancellationToken ct)
            => receiver.SubscribeAsync<RescueVehicleLocation>($"VehicleLocationShare_{rescueVehicleId}", ct);
    }
}
