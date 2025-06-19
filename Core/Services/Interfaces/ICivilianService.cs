using Core.DTO;
using Core.Models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface ICivilianService : IService<Civilian>
    {
        Task<Civilian> Add(CivilianCreateInput dto);
        Task<Civilian> Update(int id, CivilianUpdateInput dto);
        Task<string> Login(string phoneNumber, int otp);
        Task<string> SendOTP(string phoneNumber);
        Task<bool> updateCivilianStatus(int id, int civilianStatusId);
    }
}
