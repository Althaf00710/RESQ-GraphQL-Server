using Core.DTO;
using Core.Services.Interfaces;
using RESQserver_dotnet.Api.EmergencyToCivilianApi;

namespace RESQserver_dotnet.Api.EmergencyToVehicleApi
{
    [ExtendObjectType<Mutation>]
    public class EmergencyToVehicleMutation
    {
        public async Task<EmergencyToVehiclePayload> CreateEmergencyToVehicleMapping(EmergencyToVehicleInput input, [Service] IEmergencyToVehicleService service)
        {
            try
            {
                var mapping = await service.Add(input);
                return new EmergencyToVehiclePayload(true, "Mapping created successfully", mapping);
            }
            catch (Exception ex)
            {
                return new EmergencyToVehiclePayload(false, ex.Message);
            }
        }

        public async Task<EmergencyToVehiclePayload> DeleteEmergencyToVehicleMapping(int id, [Service] IEmergencyToVehicleService service)
        {
            try
            {
                var deleted = await service.Delete(id);
                return new EmergencyToVehiclePayload(
                    success: deleted,
                    message: deleted ? "Mapping deleted successfully" : "Mapping not found");
            }
            catch (Exception ex)
            {
                return new EmergencyToVehiclePayload(false, ex.Message);
            }
        }
    }
}
