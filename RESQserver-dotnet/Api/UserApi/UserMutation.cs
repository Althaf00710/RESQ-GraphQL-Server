using Core.DTO;
using Core.models;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.UserApi
{
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
    }
}
