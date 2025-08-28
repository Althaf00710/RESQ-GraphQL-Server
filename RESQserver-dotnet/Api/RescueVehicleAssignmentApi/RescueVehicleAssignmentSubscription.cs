using Application.Utils.AssignmentOffer;
using Core.DTO;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;

namespace RESQserver_dotnet.Api.RescueVehicleAssignmentApi
{
    [ExtendObjectType<Subscription>]
    public class RescueVehicleAssignmentSubscription
    {
        // Subscribes to a topic computed from the field args (vehicleId).
        [Subscribe(With = nameof(SubscribeToVehicleAssignment))]
        public VehicleAssignmentOffer OnVehicleAssignmentOffered(
            [EventMessage] VehicleAssignmentOffer offer) => offer;

        // IMPORTANT: This must return an event stream (not a string)
        public static ValueTask<ISourceStream<VehicleAssignmentOffer>> SubscribeToVehicleAssignment(
            int vehicleId,
            [Service] ITopicEventReceiver receiver)
        {
            var topic = AssignmentTopics.Vehicle(vehicleId);
            return receiver.SubscribeAsync<VehicleAssignmentOffer>(topic);
        }

        public static string TopicFor(int vehicleId) => $"VehicleAssignment_{vehicleId}";
    }
}
