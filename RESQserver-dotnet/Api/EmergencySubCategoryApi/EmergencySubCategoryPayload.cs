using Core.Models;

namespace RESQserver_dotnet.Api.EmergencySubCategoryApi
{
    public class EmergencySubCategoryPayload
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public EmergencySubCategory? EmergencySubCategory { get; set; }

        public EmergencySubCategoryPayload(bool success, string? message = null, EmergencySubCategory? emergencySubCategory = null)
        {
            Success = success;
            Message = message;
            EmergencySubCategory = emergencySubCategory;
        }
    }
}
