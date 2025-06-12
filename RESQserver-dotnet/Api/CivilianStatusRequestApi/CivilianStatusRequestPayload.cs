using Core.Models;

namespace RESQserver_dotnet.Api.CivilianStatusRequestApi
{
    public class CivilianStatusRequestPayload
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public CivilianStatusRequest? CivilianStatusRequest { get; set; }

        public CivilianStatusRequestPayload(bool success, string? message = null, CivilianStatusRequest? civilianStatusRequest = null)
        {
            Success = success;
            Message = message;
            CivilianStatusRequest = civilianStatusRequest;
        }
    }
}
