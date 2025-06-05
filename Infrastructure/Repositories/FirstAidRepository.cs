using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories
{
    public class FirstAidRepository : Repository<FirstAid>, IFirstAidRepository
    {
        private readonly AppDbContext _context;

        public FirstAidRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
