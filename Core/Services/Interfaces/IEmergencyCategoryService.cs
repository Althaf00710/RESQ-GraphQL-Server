using Core.DTO;
using Core.Models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface IEmergencyCategoryService : IService<EmergencyCategory>
    {
        Task<EmergencyCategory> Add(EmergencyCategoryCreateInput dto);
        Task<EmergencyCategory> Update(int id, EmergencyCategoryUpdateInput dto);
    }
}
