using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories
{
    public class CivilianStatusRepository : Repository<CivilianStatus>, ICivilianStatusRepository
    {
        private readonly AppDbContext _context;

        public CivilianStatusRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
