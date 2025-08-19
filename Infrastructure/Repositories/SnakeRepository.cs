using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Repositories.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class SnakeRepository : Repository<Snake>, ISnakeRepository
    {
        private readonly AppDbContext _context;
        public SnakeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> CheckExist(string scientificName)
        {
            var normalizedScientificName = scientificName.Trim().ToLower();
            return await _context.Snakes
                .AnyAsync(s => s.ScientificName.Trim().ToLower() == normalizedScientificName);
        }

        public async Task<Snake?> GetByScientificNameAsync(string scientificName)
        {
            var normalizedScientificName = scientificName.Trim().ToLower();
            return await _context.Snakes
                .FirstOrDefaultAsync(s => s.ScientificName.Trim().ToLower() == normalizedScientificName);
        }
    }
    
}
