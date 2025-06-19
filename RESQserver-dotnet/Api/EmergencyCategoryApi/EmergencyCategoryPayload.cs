using Core.Models;

namespace RESQserver_dotnet.Api.EmergencyCategoryApi
{
    public class EmergencyCategoryPayload
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public EmergencyCategory? EmergencyCategory { get; set; }

        public EmergencyCategoryPayload(bool success, string? message = null, EmergencyCategory? emergencyCategory = null)
        {
            Success = success;
            Message = message;
            EmergencyCategory = emergencyCategory;
        }
    }
}
