using Application.Services.Generic;
using AutoMapper;
using Core.DTO;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class RescueVehicleLocationService : Service<RescueVehicleLocation>, IRescueVehicleLocationService
    {
        private readonly IRescueVehicleLocationRepository _repository;
        private readonly ILogger<RescueVehicleLocationService> _logger;
        private readonly IMapper _mapper;
        public RescueVehicleLocationService(IRescueVehicleLocationRepository repository, ILogger<RescueVehicleLocationService> logger, IMapper mapper) : base(repository)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;   
        }

        public async Task<RescueVehicleLocation> GetByRescueVehicleId(int rescueVehicleId)
        {
            try
            {
                var location = await _repository.GetByRescueVehicleId(rescueVehicleId);
                if (location == null)
                {
                    _logger.LogWarning("No location found for Rescue Vehicle {rescueVehicleId}", rescueVehicleId);
                    throw new Exception($"Location for Rescue Vehicle {rescueVehicleId} not found");
                }
                return location;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting location for Rescue Vehicle {rescueVehicleId}", rescueVehicleId);
                throw;
            }
        }

        public async Task<RescueVehicleLocation> Handle(RescueVehicleLocationInput dto)
        {
            try
            {
                if (dto == null)
                {
                    _logger.LogWarning("Received null DTO for Rescue Vehicle {RescueVehicleId}", dto.RescueVehicleId);
                    throw new ArgumentNullException(nameof(dto), "DTO cannot be null");
                }

                var existing = await _repository.GetByRescueVehicleId(dto.RescueVehicleId);

                if (existing != null)
                    return await Update(dto.RescueVehicleId, dto);
                return await Add(dto);
                //summa oru try, Let's see if it's working or not
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding or updating location for Rescue Vehicle {RescueVehicleId}", dto.RescueVehicleId);
                throw;
            }
        }

        private async Task<RescueVehicleLocation> Add(RescueVehicleLocationInput dto)
        {
            try
            {
                var entity = _mapper.Map<RescueVehicleLocation>(dto);
                entity.Active = true;

                await _repository.AddAsync(entity);
                await _repository.SaveAsync();

                _logger.LogInformation("Location created for Rescue Vehicle {RescueVehicleId}", dto.RescueVehicleId);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Rescue Vehicle location");
                throw;
            }
        }

        private async Task<RescueVehicleLocation> Update(int rescueVehicleId, RescueVehicleLocationInput dto)
        {
            try
            {
                var existing = await _repository.GetByRescueVehicleId(rescueVehicleId);

                _mapper.Map(dto, existing);
                await _repository.SaveAsync();

                _logger.LogInformation("Location updated for Rescue Vehicle {RescueVehicleId}", rescueVehicleId);
                return existing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating location for Rescue Vehicle {RescueVehicleId}", rescueVehicleId);
                throw;
            }
        }
    }
}
