using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories
{
    public class CivilianTypeRepository : Repository<CivilianType>, ICivilianTypeRepository
    {
        private readonly AppDbContext _context;

        public CivilianTypeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
