using Core.DTO;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.CivilianLocationApi
{
    [ExtendObjectType<Mutation>]
    public class CivilianLocationMutation
    {
        public async Task<CivilianLocationPayload> HandleCivilianLocation(CivilianLocationInput input, [Service] ICivilianLocationService service)
        {
            try
            {
                var location = await service.Handle(input);
                return new CivilianLocationPayload(true, "Location updated successfully", location);
            }
            catch (Exception ex)
            {
                return new CivilianLocationPayload(false, $"Failed to update location: {ex.Message}");
            }
        }
    }
}
