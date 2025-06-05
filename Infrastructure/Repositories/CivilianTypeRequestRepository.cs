using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories
{
    public class CivilianTypeRequestRepository : Repository<CivilianTypeRequest>, ICivilianTypeRequestRepository
    {
        private readonly AppDbContext _context;

        public CivilianTypeRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
