using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO;
using Core.Models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface IEmergencyToCivilianService : IService<EmergencyToCivilian>
    {
        Task<EmergencyToCivilian> Add(EmergencyToCivilianInput input);
        Task<bool> Delete(int id);
        Task<IEnumerable<EmergencyToCivilian>> GetByEmergencyCategoryAsync(int emergencyCategoryId);
        Task<IEnumerable<EmergencyToCivilian>> GetByCivilianStatusAsync(int civilianStatusId);
    }
}
