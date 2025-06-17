using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.Repositories.Generic;

namespace Core.Repositories.Interfaces
{
    public interface IEmergencyCategoryRepository : IRepository<EmergencyCategory>
    {
        Task<bool> ExistAsync(string emergency, int? excludeId = null);
    }
}
