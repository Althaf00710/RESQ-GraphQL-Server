using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CivilianStatusRequestRepository : Repository<CivilianStatusRequest>, ICivilianStatusRequestRepository
    {
        private readonly AppDbContext _context;

        public CivilianStatusRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<CivilianStatusRequest> Query() =>
            _context.CivilianStatusRequests.AsNoTracking();
    }
}
