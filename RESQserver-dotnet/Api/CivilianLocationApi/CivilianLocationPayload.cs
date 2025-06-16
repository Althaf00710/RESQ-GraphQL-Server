using Core.models;
using Core.Models;

namespace RESQserver_dotnet.Api.CivilianLocationApi
{
    public class CivilianLocationPayload
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public CivilianLocation? CivilianLocation { get; set; }

        public CivilianLocationPayload(bool success, string? message = null, CivilianLocation? civilianLocation = null)
        {
            Success = success;
            Message = message;
            CivilianLocation = civilianLocation;
        }
    }
}
