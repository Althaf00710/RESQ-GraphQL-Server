using Core.Models;
using Core.Repositories.Interfaces;

namespace RESQserver_dotnet.Api.RescueVehicleAssignmentApi
{
    public class RescueVehicleAssignmentQuery
    {
        public async Task<IEnumerable<RescueVehicleAssignment>> GetAssignments([Service] IRescueVehicleAssignmentRepository repository)
        {
            return await repository.GetAllAsync();
        }
        public async Task<RescueVehicleAssignment?> GetAssignmentsById(int id, [Service] IRescueVehicleAssignmentRepository repository)
        {
            return await repository.GetByIdAsync(id);
        }
    }
}
