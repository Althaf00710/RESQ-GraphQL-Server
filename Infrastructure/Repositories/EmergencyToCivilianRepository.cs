using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EmergencyToCivilianRepository : Repository<EmergencyToCivilian>, IEmergencyToCivilianRepository
    {
        private readonly AppDbContext _context;

        public EmergencyToCivilianRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(int emergencyCategoryId, int civilianStatusId)
        {
            return await _context.EmergencyToCivilians
                .AnyAsync(etc => etc.EmergencyCategoryId == emergencyCategoryId &&
                               etc.CivilianStatusId == civilianStatusId);
        }

        public async Task<IEnumerable<EmergencyToCivilian>> GetByCivilianStatusAsync(int civilianStatusId)
        {
            return await _context.EmergencyToCivilians
                .Where(etc => etc.CivilianStatusId == civilianStatusId)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmergencyToCivilian>> GetByEmergencyCategoryAsync(int emergencyCategoryId)
        {
            return await _context.EmergencyToCivilians
                .Where(etc => etc.EmergencyCategoryId == emergencyCategoryId)
                .ToListAsync();
        }
    }
}
