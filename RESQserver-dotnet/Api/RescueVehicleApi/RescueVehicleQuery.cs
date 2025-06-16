using Core.models;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.RescueVehicleApi
{
    [ExtendObjectType<Query>]
    public class RescueVehicleQuery
    {
        public async Task<RescueVehiclePayload> GetRescueVehicleById(int id, [Service] IRescueVehicleService rescueVehicleService)
        {
            try
            {
                var rescueVehicle = await rescueVehicleService.GetByIdAsync(id);
                if (rescueVehicle == null)
                {
                    return new RescueVehiclePayload(false, "Rescue Vehicle not found");
                }
                return new RescueVehiclePayload(true, "Rescue Vehicle Found", rescueVehicle);
            }
            catch (Exception ex)
            {
                return new RescueVehiclePayload(false, ex.Message);
            }
        }
        public async Task<IEnumerable<RescueVehicle>> GetRescueVehicles([Service] IRescueVehicleService rescueVehicleService)
        {
            return await rescueVehicleService.GetAllAsync();
        }
    }
}
