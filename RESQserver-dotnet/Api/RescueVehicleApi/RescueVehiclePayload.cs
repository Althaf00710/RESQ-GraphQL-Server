using Core.models;
using Core.Models;

namespace RESQserver_dotnet.Api.RescueVehicleApi
{
    public class RescueVehiclePayload
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public RescueVehicle? RescueVehicle { get; set; }

        public RescueVehiclePayload(bool success, string? message = null, RescueVehicle? rescueVehicle = null)
        {
            Success = success;
            Message = message;
            RescueVehicle = rescueVehicle;
        }
    }
}
