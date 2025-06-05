using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories
{
    public class RescueVehicleTypeRepository : Repository<RescueVehicleType>, IRescueVehicleTypeRepository
    {
        private readonly AppDbContext _context;

        public RescueVehicleTypeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
