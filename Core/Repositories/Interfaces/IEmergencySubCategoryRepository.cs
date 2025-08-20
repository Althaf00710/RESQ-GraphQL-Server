using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.Repositories.Generic;

namespace Core.Repositories.Interfaces
{
    public interface IEmergencySubCategoryRepository : IRepository<EmergencySubCategory>
    {
        Task<bool> ExistAsync(string subCategory, int categoryId, int? excludeId = null);
        Task<List<EmergencySubCategory>> GetByCategoryIdAsync(int categoryId);
        Task<List<EmergencySubCategory>> GetWithoutFirstAidDetails();

    }
}
