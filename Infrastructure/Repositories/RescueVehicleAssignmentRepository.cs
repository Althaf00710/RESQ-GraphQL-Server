using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories
{
    public class RescueVehicleAssignmentRepository : Repository<RescueVehicleAssignment>, IRescueVehicleAssignmentRepository
    {
        private readonly AppDbContext _context;
        public RescueVehicleAssignmentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
