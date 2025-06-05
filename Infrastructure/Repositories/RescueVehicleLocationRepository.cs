using Core.models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories
{
    public class RescueVehicleLocationRepository : Repository<RescueVehicleLocation>, IRescueVehicleLocationRepository
    {
        private readonly AppDbContext _context;
        public RescueVehicleLocationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
