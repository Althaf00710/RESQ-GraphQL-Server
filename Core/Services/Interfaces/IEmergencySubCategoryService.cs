using Core.DTO;
using Core.Models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface IEmergencySubCategoryService : IService<EmergencySubCategory>
    {
        Task<EmergencySubCategory> Add(EmergencySubCategoryCreateInput dto, IFile? image);
        Task<EmergencySubCategory> Update(int id, EmergencySubCategoryUpdateInput dto, IFile? image);
        Task<List<EmergencySubCategory>> GetByCategoryIdAsync(int categoryId);
        Task<bool> CheckExist(string name, int categoryId, int? excludeId = null);
        Task<List<EmergencySubCategory>> GetWithoutFirstAidDetails();

    }
}
