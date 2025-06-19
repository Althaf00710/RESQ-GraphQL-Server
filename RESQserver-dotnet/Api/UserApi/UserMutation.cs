using Core.DTO;
using Core.Models;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.UserApi
{
    [ExtendObjectType<Mutation>]
    public class UserMutation
    {
        public async Task<UserPayload> CreateUser(UserCreateInput input, IFile? profilePicture, [Service] IUserService userService)
        {
            try
            {
                var user = await userService.Add(input, profilePicture);
                return new UserPayload(true, "User created successfully", user);
            }
            catch (Exception ex)
            {
                return new UserPayload(false, ex.Message);
            }
        }

        public async Task<UserPayload> UpdateUser(int id, UserUpdateInput input, IFile? profilePicture, [Service] IUserService userService)
        {
            try
            {
                var user = await userService.Update(id, input, profilePicture);
                return new UserPayload(true, "User updated successfully", user);
            }
            catch (Exception ex)
            {
                return new UserPayload(false, ex.Message);
            }
        }

        public async Task<UserPayload> DeleteUser(int id, [Service] IUserService userService)
        {
            try
            {
                await userService.Delete(id);
                return new UserPayload(true, "User deleted successfully");
            }
            catch (Exception ex)
            {
                return new UserPayload(false, ex.Message);
            }
        }

        public async Task<UserPayload> LoginUser(string username, string password, [Service] IUserService userService)
        {
            try
            {
                var token = await userService.Login(username, password);

                if (token == null) return new UserPayload(false, "Invalid Username or Password");
                
                return new UserPayload(true, token);
            }
            catch (Exception ex)
            {
                return new UserPayload(false, ex.Message);
            }
        }

    }
}
