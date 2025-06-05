using Core.models;

namespace RESQserver_dotnet.Api.UserApi
{
    public class UserPayload
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public User? User { get; set; }

        public UserPayload(bool success, string? message = null, User? user = null)
        {
            Success = success;
            Message = message;
            User = user;
        }
    }
    
}
