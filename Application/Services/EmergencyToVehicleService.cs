using Application.Services.Generic;
using AutoMapper;
using Core.DTO;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EmergencyToVehicleService : Service<EmergencyToVehicle>, IEmergencyToVehicleService
    {
        private readonly IEmergencyToVehicleRepository _repository;
        private readonly ILogger<EmergencyToVehicleService> _logger;
        private readonly IMapper _mapper;

        public EmergencyToVehicleService(IEmergencyToVehicleRepository repository, ILogger<EmergencyToVehicleService> logger, IMapper mapper) : base(repository)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<EmergencyToVehicle> Add(EmergencyToVehicleInput input)
        {
            if (await _repository.ExistsAsync(input.EmergencyCategoryId, input.VehicleCategoryId))
            {
                _logger.LogWarning("Mapping already exists between emergency category {CategoryId} and vehicle category {VehicleCategoryId}",
                    input.EmergencyCategoryId, input.VehicleCategoryId);
                throw new InvalidOperationException("This mapping already exists");
            }

            var entity = _mapper.Map<EmergencyToVehicle>(input);
            await _repository.AddAsync(entity);
            await _repository.SaveAsync();

            _logger.LogInformation("Created new emergency-to-vehicle mapping: {MappingId}", entity.Id);
            return entity;
        }

        public async Task<IEnumerable<EmergencyToVehicle>> GetByEmergencyCategoryAsync(int emergencyCategoryId)
        {
            return await _repository.GetByEmergencyCategoryAsync(emergencyCategoryId);
        }

        public async Task<IEnumerable<EmergencyToVehicle>> GetByVehicleCategoryAsync(int vehicleCategoryId)
        {
            return await _repository.GetByVehicleCategoryAsync(vehicleCategoryId);
        }
    }
}