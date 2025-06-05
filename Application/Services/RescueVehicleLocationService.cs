using Application.Services.Generic;
using Core.models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace Application.Services
{
    public class RescueVehicleLocationService : Service<RescueVehicleLocation>, IRescueVehicleLocationService
    {
        private readonly IRescueVehicleLocationRepository _repository;
        public RescueVehicleLocationService(IRescueVehicleLocationRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
