using Application.Services.Generic;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace Application.Services
{
    public class RescueVehicleTypeService : Service<RescueVehicleType>, IRescueVehicleTypeService
    {
        private readonly IRescueVehicleTypeRepository _repository;
        public RescueVehicleTypeService(IRescueVehicleTypeRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
