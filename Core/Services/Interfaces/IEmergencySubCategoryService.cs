using Core.DTO;
using Core.Models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface IEmergencySubCategoryService : IService<EmergencySubCategory>
    {
        Task<EmergencySubCategory> Add(EmergencySubCategoryCreateInput dto, IFile? image);
        Task<EmergencySubCategory> Update(int id, EmergencySubCategoryUpdateInput dto, IFile? image);

    }
}
