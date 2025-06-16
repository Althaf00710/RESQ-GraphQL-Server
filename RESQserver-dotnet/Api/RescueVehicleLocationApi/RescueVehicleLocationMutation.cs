using Core.DTO;
using Core.Services.Interfaces;
using RESQserver_dotnet.Api.CivilianLocationApi;

namespace RESQserver_dotnet.Api.RescueVehicleLocationApi
{
    [ExtendObjectType<Mutation>]
    public class RescueVehicleLocationMutation
    {
        public async Task<RescueVehicleLocationPayload> HandleRescueVehicleLocation(RescueVehicleLocationInput input, [Service] IRescueVehicleLocationService service)
        {
            try
            {
                var location = await service.Handle(input);
                return new RescueVehicleLocationPayload(true, "Location updated successfully", location);
            }
            catch (Exception ex)
            {
                return new RescueVehicleLocationPayload(false, $"Failed to update location: {ex.Message}");
            }
        }
    }
}
