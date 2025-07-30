using Core.DTO;
using Core.Models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface ICivilianStatusService : IService<CivilianStatus>
    {
        Task<CivilianStatus> Add(CivilianStatusCreateInput dto);
        Task<CivilianStatus> Update(int id, CivilianStatusUpdateInput dto);
        Task<bool> RoleExistAsync(string role, int? excludeId = null);


    }
}
