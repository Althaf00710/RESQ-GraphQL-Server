
namespace Core.DTO
{
    public class UserCreateInput
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class UserUpdateInput
    {
        public string? Name { get; set; }
        public string? Email { get; set; } 
        public string? Username { get; set; }
        public string? Password { get; set; } 
    }

    

}
