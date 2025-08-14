using Core.DTO;
using Core.Models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface ICivilianStatusRequestService : IService<CivilianStatusRequest>
    {
        Task<CivilianStatusRequest> Add(CivilianStatusRequestCreateInput dto, IFile? proofPicture);
        Task<CivilianStatusRequest> Update(int id, CivilianStatusRequestUpdateInput dto);
        IQueryable<CivilianStatusRequest> Query();
    }
}
