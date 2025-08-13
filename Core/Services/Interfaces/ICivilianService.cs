using Core.DTO;
using Core.Models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface ICivilianService : IService<Civilian>
    {
        Task<Civilian> Add(CivilianCreateInput dto);
        Task<Civilian> Update(int id, CivilianUpdateInput dto);
        Task<CivilianLogin> Login(string phoneNumber, int otp);
        Task<bool> SendOTP(string phoneNumber);
        Task<bool> updateCivilianStatus(int id, int civilianStatusId);
        Task<bool> Restrict(int id);
        Task<bool> Unrestrict(int id);
        IQueryable<Civilian> Query();
    }
}
