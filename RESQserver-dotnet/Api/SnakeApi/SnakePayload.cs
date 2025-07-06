using Core.Models;

namespace RESQserver_dotnet.Api.SnakeApi
{
    public class SnakePayload
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public Snake? Snake { get; set; }

        public SnakePayload(bool success, string? message = null, Snake? snake = null)
        {
            Success = success;
            Message = message;
            Snake = snake;
        }
    }
}
