using Core.Models;

namespace RESQserver_dotnet.Api.RescueVehicleRequestApi
{
    public class RescueVehicleRequestPayload
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public RescueVehicleRequest? RescueVehicleRequest { get; set; }

        public RescueVehicleRequestPayload(bool success, string? message = null, RescueVehicleRequest? rescueVehicleRequest = null)
        {
            Success = success;
            Message = message;
            RescueVehicleRequest = rescueVehicleRequest;
        }
    }
}
