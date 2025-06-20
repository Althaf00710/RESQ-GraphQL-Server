using Core.Models;

namespace RESQserver_dotnet.Api.EmergencyToCivilianApi
{
    public class EmergencyToCivilianPayload
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public EmergencyToCivilian? EmergencyToCivilian { get; set; }

        public EmergencyToCivilianPayload(bool success, string? message = null, EmergencyToCivilian? emergencyToCivilian = null)
        {
            Success = success;
            Message = message;
            EmergencyToCivilian = emergencyToCivilian;
        }
    }
}
