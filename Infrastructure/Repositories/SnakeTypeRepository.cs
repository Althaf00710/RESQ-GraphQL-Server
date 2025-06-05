using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Repositories.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories
{
    internal class SnakeTypeRepository : Repository<SnakeType>, ISnakeTypeRepository
    {
        private readonly AppDbContext _context;
        public SnakeTypeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
    
}
