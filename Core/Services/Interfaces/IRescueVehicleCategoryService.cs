using Core.DTO;
using Core.Models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface IRescueVehicleCategoryService : IService<RescueVehicleCategory>
    {
        Task<RescueVehicleCategory> Add(RescueVehicleCategoryCreateInput dto);
        Task<RescueVehicleCategory> Update(int id, RescueVehicleCategoryUpdateInput dto);
    }
}
