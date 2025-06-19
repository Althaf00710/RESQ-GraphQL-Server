using Core.Models;

namespace RESQserver_dotnet.Api.FirstAidDetailApi
{
    public class FirstAidDetailPayload
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public FirstAidDetail? FirstAidDetail { get; set; }

        public FirstAidDetailPayload(bool success, string? message = null, FirstAidDetail? firstAidDetail = null)
        {
            Success = success;
            Message = message;
            FirstAidDetail = firstAidDetail;
        }
    }
}
