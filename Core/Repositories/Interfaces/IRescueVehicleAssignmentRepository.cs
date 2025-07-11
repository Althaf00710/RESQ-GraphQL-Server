using Core.Models;
using Core.Repositories.Generic;

namespace Core.Repositories.Interfaces
{
    public interface IRescueVehicleAssignmentRepository : IRepository<RescueVehicleAssignment>
    {
        Task<RescueVehicleAssignment?> GetActiveAssignmentAsync(int vehicleId);
    }
}
