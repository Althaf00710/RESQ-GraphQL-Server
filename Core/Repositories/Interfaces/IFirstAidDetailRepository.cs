using Core.Models;
using Core.Repositories.Generic;

namespace Core.Repositories.Interfaces
{
    public interface IFirstAidDetailRepository : IRepository<FirstAidDetail>
    {
        Task<bool> ExistsAsync(int emergencySubCategoryId, string point, int? excludeId = null);
        Task<IEnumerable<FirstAidDetail>> GetBySubCategoryIdAsync(int emergencySubCategoryId);
    }
}
