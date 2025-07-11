using Core.DTO;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.RescueVehicleAssignmentApi
{
    [ExtendObjectType<Mutation>]
    public class RescueVehicleAssignmentMutation
    {
        public async Task<RescueVehicleAssignmentPayload> CreateRescueVehicleAssignment(AssignmentCreateInput input, [Service] IRescueVehicleAssignmentService service)
        {
            try
            {
                var assignment = await service.Add(input);
                return new RescueVehicleAssignmentPayload(true, "Rescue vehicle assignment created successfully", assignment);
            }
            catch (InvalidOperationException ex)
            {
                return new RescueVehicleAssignmentPayload(false, ex.Message);
            }
            catch (Exception ex)
            {
                return new RescueVehicleAssignmentPayload(false, ex.Message);
            }
        }

        public async Task<RescueVehicleAssignmentPayload> UpdateRescueVehicleAssignment(int id, AssignmentUpdateInput input, [Service] IRescueVehicleAssignmentService service)
        {
            try
            {
                var assignment = await service.Update(id, input);
                return new RescueVehicleAssignmentPayload(true, "Rescue vehicle assignment updated successfully", assignment);
            }
            catch (InvalidOperationException ex)
            {
                return new RescueVehicleAssignmentPayload(false, ex.Message);
            }
            catch (Exception ex)
            {
                return new RescueVehicleAssignmentPayload(false, ex.Message);
            }
        }

        public async Task<RescueVehicleAssignmentPayload> CancelRescueVehicleAssignment(int id, [Service] IRescueVehicleAssignmentService service)
        {
            try
            {
                var assignment = await service.Update(id, new AssignmentUpdateInput { Status = "Cancelled" });
                return new RescueVehicleAssignmentPayload(true, "Rescue vehicle assignment cancelled successfully", assignment);
            }
            catch (InvalidOperationException ex)
            {
                return new RescueVehicleAssignmentPayload(false, ex.Message);
            }
            catch (Exception ex)
            {
                return new RescueVehicleAssignmentPayload(false, ex.Message);
            }
        }
    }
}
