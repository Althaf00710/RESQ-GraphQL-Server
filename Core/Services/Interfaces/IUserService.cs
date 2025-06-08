using Core.DTO;
using Core.models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface IUserService : IService<User>
    {
        Task<User> Add(UserCreateInput dto, IFile? profilePicture);
        Task<User> Update(int id, UserUpdateInput dto, IFile? profilePicture);
        Task<string> Login(string username, string password);
    }
}
