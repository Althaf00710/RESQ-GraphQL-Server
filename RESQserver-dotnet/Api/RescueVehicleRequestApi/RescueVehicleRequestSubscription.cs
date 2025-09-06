using Core.DTO;
using Core.Models;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;

namespace RESQserver_dotnet.Api.RescueVehicleRequestApi
{
    [ExtendObjectType<Subscription>]
    public class RescueVehicleRequestSubscription
    {
        [Subscribe]
        [Topic("VehicleRequestStatusChanged")]
        public RescueVehicleRequest OnRescueVehicleRequestStatusChangedBroadcast([EventMessage] RescueVehicleRequest request) => request;

        [Subscribe(With = nameof(SubscribeToRequestStatusAsync))]
        public RescueVehicleRequest OnRescueVehicleRequestStatusChanged(
            int requestId,
            [EventMessage] RescueVehicleRequest request) => request;

        // Builds the source stream for a specific requestId
        public ValueTask<ISourceStream<RescueVehicleRequest>> SubscribeToRequestStatusAsync(
            int requestId,
            [Service] ITopicEventReceiver receiver,
            CancellationToken ct)
            => receiver.SubscribeAsync<RescueVehicleRequest>(
                $"VehicleRequestStatusChanged_{requestId}", ct);
    }
}
