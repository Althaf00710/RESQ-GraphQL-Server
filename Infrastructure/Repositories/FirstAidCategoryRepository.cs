using Core.models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories
{
    public class FirstAidCategoryRepository : Repository<FirstAidCategory>, IFirstAidCategoryRepository
    {
        private readonly AppDbContext _context;

        public FirstAidCategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
