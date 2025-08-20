using Core.Models;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.EmergencySubCategoryApi
{
    [ExtendObjectType<Query>]
    public class EmergencySubCategoryQuery
    {
        public async Task<IEnumerable<EmergencySubCategory>> GetEmergencySubCategories([Service] IEmergencySubCategoryService service)
        {
            return await service.GetAllAsync();
        }

        public async Task<EmergencySubCategory?> GetEmergencySubCategoryById(int id, [Service] IEmergencySubCategoryService service)
        {
            return await service.GetByIdAsync(id);
        }

        public async Task<IEnumerable<EmergencySubCategory>> GetSubCategoriesByCategoryId(int categoryId, [Service] IEmergencySubCategoryService service)
        {
            return await service.GetByCategoryIdAsync(categoryId);
        }

        public async Task<bool> EmergencySubCategoryValidate(string name, int categoryId, [Service] IEmergencySubCategoryService service,int? excludeId = null)
        {
            return await service.CheckExist(name, categoryId, excludeId);
        }

        public async Task<List<EmergencySubCategory>> GetEmergencySubCategoryWithoutFirstAid([Service] IEmergencySubCategoryService service)
        {
            return await service.GetWithoutFirstAidDetails();
        }
    }
}
