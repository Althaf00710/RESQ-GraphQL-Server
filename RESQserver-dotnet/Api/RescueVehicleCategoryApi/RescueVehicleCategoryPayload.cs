using Core.Models;

namespace RESQserver_dotnet.Api.RescueVehicleCategoryApi
{
    public class RescueVehicleCategoryPayload
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public RescueVehicleCategory? RescueVehicleCategory { get; set; }

        public RescueVehicleCategoryPayload(bool success, string? message = null, RescueVehicleCategory? rescueVehicleCategory = null)
        {
            Success = success;
            Message = message;
            RescueVehicleCategory = rescueVehicleCategory;
        }
    }
}
