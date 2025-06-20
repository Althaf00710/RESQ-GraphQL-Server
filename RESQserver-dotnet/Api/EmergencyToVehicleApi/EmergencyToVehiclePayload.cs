using Core.Models;

namespace RESQserver_dotnet.Api.EmergencyToVehicleApi
{
    public class EmergencyToVehiclePayload
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public EmergencyToVehicle? EmergencyToVehicle { get; set; }

        public EmergencyToVehiclePayload(bool success, string? message = null, EmergencyToVehicle? emergencyToVehicle = null)
        {
            Success = success;
            Message = message;
            EmergencyToVehicle = emergencyToVehicle;
        }
    }
}
