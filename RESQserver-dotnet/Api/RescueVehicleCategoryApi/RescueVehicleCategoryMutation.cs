using Core.DTO;
using Core.Services.Interfaces;
using RESQserver_dotnet.Api.CivilianStatusApi;

namespace RESQserver_dotnet.Api.RescueVehicleCategoryApi
{
    [ExtendObjectType<Mutation>]
    public class RescueVehicleCategoryMutation
    {
        public async Task<RescueVehicleCategoryPayload> CreateRescueVehicleCategory(RescueVehicleCategoryCreateInput input, [Service] IRescueVehicleCategoryService service)
        {
            try
            {
                var created = await service.Add(input);
                return new RescueVehicleCategoryPayload(true, "Rescue vehicle category created successfully", created);
            }
            catch (Exception ex)
            {
                return new RescueVehicleCategoryPayload(false, $"Failed to create rescue vehicle category: {ex.Message}");
            }
        }

        public async Task<RescueVehicleCategoryPayload> UpdateRescueVehicleCategory(
            int id,
            RescueVehicleCategoryUpdateInput input,
            [Service] IRescueVehicleCategoryService service)
        {
            try
            {
                var updated = await service.Update(id, input);
                return new RescueVehicleCategoryPayload(true, "Rescue vehicle category updated successfully", updated);
            }
            catch (Exception ex)
            {
                return new RescueVehicleCategoryPayload(false, $"Failed to update rescue vehicle category: {ex.Message}");
            }
        }

        public async Task<RescueVehicleCategoryPayload> DeleteRescueVehicleCategory(
            int id,
            [Service] IRescueVehicleCategoryService service)
        {
            try
            {
                var deleted = await service.Delete(id);
                if (!deleted)
                    return new RescueVehicleCategoryPayload(false, $"Rescue vehicle category with ID {id} not found");

                return new RescueVehicleCategoryPayload(true, "Rescue vehicle category deleted successfully");
            }
            catch (Exception ex)
            {
                return new RescueVehicleCategoryPayload(false, $"Failed to delete rescue vehicle category: {ex.Message}");
            }
        }
    }
}
