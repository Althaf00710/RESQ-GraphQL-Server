using Core.DTO;
using Core.Models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface IUserService : IService<User>
    {
        Task<User> Add(UserCreateInput dto, IFile? profilePicture);
        Task<User> Update(int id, UserUpdateInput dto, IFile? profilePicture);
        Task<UserLogin> Login(string username, string password);
        Task<bool> CheckUsernameExistsAsync(string username);
        Task<bool> CheckEmailExistsAsync(string email);
    }
}
