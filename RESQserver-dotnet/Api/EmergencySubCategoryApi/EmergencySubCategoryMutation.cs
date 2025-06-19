using Core.DTO;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.EmergencySubCategoryApi
{
    [ExtendObjectType<Mutation>]
    public class EmergencySubCategoryMutation
    {
        public async Task<EmergencySubCategoryPayload> CreateEmergencySubCategory(EmergencySubCategoryCreateInput input, IFile? image, [Service] IEmergencySubCategoryService service)
        {
            try
            {
                var created = await service.Add(input, image);
                return new EmergencySubCategoryPayload(true, "Emergency sub category created successfully", created);
            }
            catch (Exception ex)
            {
                return new EmergencySubCategoryPayload(false, $"Failed to create emergency sub category: {ex.Message}");
            }
        }
        public async Task<EmergencySubCategoryPayload> UpdateEmergencySubCategory(int id, EmergencySubCategoryUpdateInput input, IFile? image, [Service] IEmergencySubCategoryService service)
        {
            try
            {
                var updated = await service.Update(id, input, image);
                return new EmergencySubCategoryPayload(true, "Emergency sub category updated successfully", updated);
            }
            catch (Exception ex)
            {
                return new EmergencySubCategoryPayload(false, $"Failed to update emergency sub-category: {ex.Message}");
            }
        }
        public async Task<EmergencySubCategoryPayload> DeleteEmergencySubCategory(int id, [Service] IEmergencySubCategoryService service)
        {
            try
            {
                var deleted = await service.Delete(id);
                if (!deleted)
                    return new EmergencySubCategoryPayload(false, $"Emergency sub category with ID {id} not found");
                return new EmergencySubCategoryPayload(true, "Emergency sub category deleted successfully");
            }
            catch (Exception ex)
            {
                return new EmergencySubCategoryPayload(false, $"Failed to delete emergency sub-category: {ex.Message}");
            }
        }
    }
}
