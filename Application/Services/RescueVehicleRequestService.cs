using Application.Services.Generic;
using Core.models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace Application.Services
{
    public class RescueVehicleRequestService : Service<RescueVehicleRequest>, IRescueVehicleRequestService
    {
        private readonly IRescueVehicleRequestRepository _repository;
        public RescueVehicleRequestService(IRescueVehicleRequestRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
