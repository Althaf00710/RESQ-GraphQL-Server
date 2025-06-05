using Core.Models;
using Core.Repositories.Generic;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories
{
    public class CivilianLocationRepository : Repository<CivilianLocation>, ICivilianLocationRepository
    {
        private readonly AppDbContext _context;

        public CivilianLocationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
