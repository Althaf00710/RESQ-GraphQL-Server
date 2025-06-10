using Core.DTO;
using Core.models;
using Core.Models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface ICivilianStatusService : IService<CivilianStatus>
    {
        Task<CivilianStatus> Add(CivilianStatusCreateInput dto);
        Task<CivilianStatus> Update(int id, CivilianStatusUpdateInput dto);
        
    }
}
