using Core.Models;
using Core.Repositories.Generic;

namespace Core.Repositories.Interfaces
{
    public interface ICivilianRepository : IRepository<Civilian>
    {
        Task<Civilian?> GetByPhoneNumberAsync(string phoneNumber);
        Task<Civilian?> GetByEmailAsync(string email);
        Task<Civilian?> GetByNICAsync(string nic);
    }
}
