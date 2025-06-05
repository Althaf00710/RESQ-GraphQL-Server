using Core.models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories
{
    public class RescueVehicleRequestRepository : Repository<RescueVehicleRequest>, IRescueVehicleRequestRepository
    {
        private readonly AppDbContext _context;

        public RescueVehicleRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
