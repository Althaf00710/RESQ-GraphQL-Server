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
    public class EmergencySubCategoryRepository : Repository<EmergencySubCategory> , IEmergencySubCategoryRepository
    {
        private readonly AppDbContext _context;

        public EmergencySubCategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<bool> ExistAsync(string subCategory, int categoryId, int? excludeId = null)
        {
            var query = _context.EmergencySubCategories
                .Where(esc => esc.Name == subCategory && esc.EmergencyCategoryId == categoryId);

            if (excludeId.HasValue)
            {
                query = query.Where(esc => esc.Id != excludeId.Value);
            }

            return query.AnyAsync();
        }
    }
}
