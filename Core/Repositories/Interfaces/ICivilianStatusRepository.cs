using Core.Models;
using Core.Repositories.Generic;

namespace Core.Repositories.Interfaces
{
    public interface ICivilianStatusRepository : IRepository<CivilianStatus>
    {
        Task<bool> RoleExistsAsync (string role, int? excludeId = null);
    }
}
