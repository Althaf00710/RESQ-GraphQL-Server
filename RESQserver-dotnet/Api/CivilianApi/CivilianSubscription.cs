using Core.DTO;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using static Application.Services.CivilianLocationService;

namespace RESQserver_dotnet.Api.CivilianApi
{
    [ExtendObjectType<Subscription>]
    public class CivilianSubscription
    {
        [Subscribe(With = nameof(SubscribeToNearbyEmergencyAsync))]
        public NearbyEmergencyPayload OnNearbyEmergency(
        [EventMessage] NearbyEmergencyPayload payload) => payload;

        // Provides the source stream (typed):
        private ValueTask<ISourceStream<NearbyEmergencyPayload>> SubscribeToNearbyEmergencyAsync(
            int civilianId,
            [Service] ITopicEventReceiver receiver)
            => receiver.SubscribeAsync<NearbyEmergencyPayload>(Topics.CivilianNearby(civilianId));
    }
}
