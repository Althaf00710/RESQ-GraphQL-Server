using Core.models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories
{
    public class RescueVehicleRepository : Repository<RescueVehicle>, IRescueVehicleRepository
    {
        private readonly AppDbContext _context;
        public RescueVehicleRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
