using Core.Models;

namespace RESQserver_dotnet.Api.RescueVehicleAssignmentApi
{
    public class RescueVehicleAssignmentPayload
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public RescueVehicleAssignment? RescueVehicleAssignment { get; set; }

        public RescueVehicleAssignmentPayload(bool success, string? message = null, RescueVehicleAssignment? rescueVehicleAssignment = null)
        {
            Success = success;
            Message = message;
            RescueVehicleAssignment = rescueVehicleAssignment;
        }
    }
}
