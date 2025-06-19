using Core.DTO;
using Core.Models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface IFirstAidDetailService : IService<FirstAidDetail>
    {
        Task<FirstAidDetail> Add(FirstAidDetailCreateInput dto, IFile? image);
        Task<FirstAidDetail> Update(int id, FirstAidDetailUpdateInput dto, IFile? image);
        Task<IEnumerable<FirstAidDetail>> GetBySubCategoryIdAsync(int emergencySubCategoryId);
    }
}
