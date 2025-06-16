using Core.models;
using Core.Models;

namespace RESQserver_dotnet.Api.RescueVehicleLocationApi
{
    public class RescueVehicleLocationPayload
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public RescueVehicleLocation? RescueVehicleLocation { get; set; }

        public RescueVehicleLocationPayload(bool success, string? message = null, RescueVehicleLocation? rescueVehicleLocation = null)
        {
            Success = success;
            Message = message;
            RescueVehicleLocation = rescueVehicleLocation;
        }
    }
}
