using Application.Services.Generic;
using Core.models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace Application.Services
{
    public class RescueVehicleService : Service<RescueVehicle>, IRescueVehicleService
    {
        private readonly IRescueVehicleRepository _repository;

        public RescueVehicleService(IRescueVehicleRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
