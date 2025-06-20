using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.Repositories.Generic;

namespace Core.Repositories.Interfaces
{
    public interface IEmergencyToCivilianRepository : IRepository<EmergencyToCivilian>
    {
        Task<bool> ExistsAsync(int emergencyCategoryId, int civilianStatusId);
        Task<IEnumerable<EmergencyToCivilian>> GetByEmergencyCategoryAsync(int emergencyCategoryId);
        Task<IEnumerable<EmergencyToCivilian>> GetByCivilianStatusAsync(int civilianStatusId);
    }
}
