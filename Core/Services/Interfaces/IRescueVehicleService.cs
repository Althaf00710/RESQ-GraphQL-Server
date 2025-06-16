using Core.DTO;
using Core.models;
using Core.Models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface IRescueVehicleService : IService<RescueVehicle>
    {
        Task<RescueVehicle> Add(RescueVehicleCreateInput dto);
        Task<RescueVehicle> Update(int id, RescueVehicleUpdateInput dto);
    }
}
