using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories
{
    public class CivilianRepository : Repository<Civilian>, ICivilianRepository
    {
        private readonly AppDbContext _context;

        public CivilianRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
