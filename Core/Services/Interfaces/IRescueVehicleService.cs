using Core.DTO;
using Core.Models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface IRescueVehicleService : IService<RescueVehicle>
    {
        Task<RescueVehicle> Add(RescueVehicleCreateInput dto);
        Task<RescueVehicle> Update(int id, RescueVehicleUpdateInput dto);
        IQueryable<RescueVehicle> Query();
        Task<RescueVehicleLogin> Login(string plateNumber, string password);
    }
}
