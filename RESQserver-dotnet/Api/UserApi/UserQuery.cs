using Core.Models;
using Core.Services.Interfaces;
using HotChocolate.Authorization;

namespace RESQserver_dotnet.Api.UserApi
{
    [ExtendObjectType<Query>]
    public class UserQuery
    {
        public async Task<IEnumerable<User>> GetUsers([Service] IUserService userService)
            => await userService.GetAllAsync();

        public async Task<User?> GetUserById(int id, [Service] IUserService userService)
            => await userService.GetByIdAsync(id);

        public async Task<bool?> UserCheckUsername(string username, [Service] IUserService userService)
            => await userService.CheckUsernameExistsAsync(username);

        public async Task<bool?> UserCheckEmail(string email, [Service] IUserService userService)
            => await userService.CheckEmailExistsAsync(email);

    }
}
