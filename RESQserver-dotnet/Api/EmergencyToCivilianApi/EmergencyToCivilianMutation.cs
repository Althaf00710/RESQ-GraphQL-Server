using Core.DTO;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.EmergencyToCivilianApi
{
    [ExtendObjectType<Mutation>]
    public class EmergencyToCivilianMutation
    {
        public async Task<EmergencyToCivilianPayload> CreateEmergencyToCivilianMapping(EmergencyToCivilianInput input, [Service] IEmergencyToCivilianService service)
        {
            try
            {
                var mapping = await service.Add(input);
                return new EmergencyToCivilianPayload(true, "Mapping created successfully", mapping);
            }
            catch (Exception ex)
            {
                return new EmergencyToCivilianPayload(false, ex.Message);
            }
        }

        public async Task<EmergencyToCivilianPayload> DeleteEmergencyToCivilianMapping(int id, [Service] IEmergencyToCivilianService service)
        {
            try
            {
                var deleted = await service.Delete(id);
                return new EmergencyToCivilianPayload(
                    success: deleted,
                    message: deleted ? "Mapping deleted successfully" : "Mapping not found");
            }
            catch (Exception ex)
            {
                return new EmergencyToCivilianPayload(false, ex.Message);
            }
        }

    }
}
