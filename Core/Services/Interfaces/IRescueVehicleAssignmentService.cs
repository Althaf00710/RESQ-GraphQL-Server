using Core.DTO;
using Core.Models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface IRescueVehicleAssignmentService : IService<RescueVehicleAssignment>
    {
        Task<RescueVehicleAssignment> Add(AssignmentCreateInput dto);
        Task<RescueVehicleAssignment> Update(int id, AssignmentUpdateInput dto);
    }
}
