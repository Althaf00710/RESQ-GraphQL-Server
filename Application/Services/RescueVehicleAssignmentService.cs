using Application.Services.Generic;
using Core.models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace Application.Services
{
    public class RescueVehicleAssignmentService : Service<RescueVehicleAssignment>, IRescueVehicleAssignmentService
    {
        private readonly IRescueVehicleAssignmentRepository _repository;
        public RescueVehicleAssignmentService(IRescueVehicleAssignmentRepository repository) : base(repository)
        {
            _repository = repository;
        }

    }
}
