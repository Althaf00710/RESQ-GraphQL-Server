using Application.Services;
using Core.Models;

namespace RESQserver_dotnet.Api.CivilianLocationApi
{
    [ExtendObjectType<Subscription>]
    public class CivilianLocationSubscription
    {
        [Subscribe]
        [Topic(CivilianLocationService.Topic)]
        public CivilianLocation OnCivilianLocationShare([EventMessage] CivilianLocation location) => location;
    }
}
