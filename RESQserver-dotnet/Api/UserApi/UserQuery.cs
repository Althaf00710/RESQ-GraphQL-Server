using Core.models;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.UserApi
{
    public class UserQuery
    {
        public async Task<IEnumerable<User>> GetUsers([Service] IUserService userService)
            => await userService.GetAllAsync();

        public async Task<User?> GetUserById(int id, [Service] IUserService userService)
            => await userService.GetByIdAsync(id);

    }
}
