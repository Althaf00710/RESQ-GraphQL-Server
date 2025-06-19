using Core.DTO;
using Core.Models;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.EmergencyCategoryApi
{
    [ExtendObjectType<Mutation>]
    public class EmergencyCategoryMutation
    {
        public async Task<EmergencyCategoryPayload> CreateEmergencyCategory(
            EmergencyCategoryCreateInput input,
            [Service] IEmergencyCategoryService service)
        {
            try
            {
                var created = await service.Add(input);
                return new EmergencyCategoryPayload(true, "Emergency category created successfully", created);
            }
            catch (Exception ex)
            {
                return new EmergencyCategoryPayload(false, $"Failed to create emergency category: {ex.Message}");
            }
        }

        public async Task<EmergencyCategoryPayload> UpdateEmergencyCategory(
            int id,
            EmergencyCategoryUpdateInput input,
            [Service] IEmergencyCategoryService service)
        {
            try
            {
                var updated = await service.Update(id, input);
                return new EmergencyCategoryPayload(true, "Emergency category updated successfully", updated);
            }
            catch (Exception ex)
            {
                return new EmergencyCategoryPayload(false, $"Failed to update emergency category: {ex.Message}");
            }
        }

        public async Task<EmergencyCategoryPayload> DeleteEmergencyCategory(
            int id,
            [Service] IEmergencyCategoryService service)
        {
            try
            {
                var deleted = await service.Delete(id);
                if (!deleted)
                    return new EmergencyCategoryPayload(false, $"Emergency category with ID {id} not found");

                return new EmergencyCategoryPayload(true, "Emergency category deleted successfully");
            }
            catch (Exception ex)
            {
                return new EmergencyCategoryPayload(false, $"Failed to delete emergency category: {ex.Message}");
            }
        }
    }
}
