using Core.Models;

namespace RESQserver_dotnet.Api.CivilianApi
{
    public class CivilianPayload
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public Civilian? Civilian { get; set; }

        public CivilianPayload(bool success, string? message = null, Civilian? civilian = null)
        {
            Success = success;
            Message = message;
            Civilian = civilian;
        }
    }
}
