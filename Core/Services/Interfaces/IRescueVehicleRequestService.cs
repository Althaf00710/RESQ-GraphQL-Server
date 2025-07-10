using Core.DTO;
using Core.Models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface IRescueVehicleRequestService : IService<RescueVehicleRequest>
    {
        Task<RescueVehicleRequest> Add(RescueVehicleRequestCreateInput dto);
        Task<RescueVehicleRequest> Update(int id, RescueVehicleRequestUpdateInput dto);
    }
}
