using Core.models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories
{
    public class FirstAidDetailRepository : Repository<FirstAidDetail>, IFirstAidDetailRepository
    {
        private readonly AppDbContext _context;

        public FirstAidDetailRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
