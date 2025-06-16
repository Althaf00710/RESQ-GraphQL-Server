using Application.Services.Generic;
using Application.Utils;
using AutoMapper;
using Core.DTO;
using Core.models;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class RescueVehicleService : Service<RescueVehicle>, IRescueVehicleService
    {
        private readonly IRescueVehicleRepository _repository;
        private readonly IRescueVehicleCategoryRepository _categoryRepository;
        private readonly ILogger<RescueVehicleService> _logger;
        private readonly IMapper _mapper;

        public RescueVehicleService(IRescueVehicleRepository repository, IRescueVehicleCategoryRepository categoryRepository, ILogger<RescueVehicleService> logger, IMapper mapper) : base(repository)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<RescueVehicle> Add(RescueVehicleCreateInput dto)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(dto.RescueVehicleCategoryId);
                if (category == null)
                {
                    _logger.LogWarning("Rescue vehicle category with ID {CategoryId} not found", dto.RescueVehicleCategoryId);
                    throw new Exception("Rescue vehicle category not found");
                }

                var vehicle = _mapper.Map<RescueVehicle>(dto);
                vehicle.Code = await GenerateVehicleCode(category);
                vehicle.Password = PasswordHash.HashPassword(dto.Password);

                await _repository.AddAsync(vehicle);
                await _repository.SaveAsync();

                _logger.LogInformation("Rescue vehicle created successfully with code {VehicleCode}", vehicle.Code);

                return vehicle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating rescue vehicle");
                throw;
            }
        }

        public async Task<RescueVehicle> Update(int id, RescueVehicleUpdateInput dto)
        {
            try
            {
                var vehicle = await _repository.GetByIdAsync(id);
                if (vehicle == null)
                {
                    throw new Exception($"Rescue vehicle with ID {id} not found");
                }

                _mapper.Map(dto, vehicle);
                await _repository.SaveAsync();

                _logger.LogInformation("Rescue vehicle {VehicleCode} updated successfully", vehicle.Code);
                return vehicle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating rescue vehicle with ID {VehicleId}", id);
                throw;
            }
        }

        private async Task<string> GenerateVehicleCode(RescueVehicleCategory category)
        {
            string prefix = category.Name[0].ToString().ToUpper();
            int nextNumber = await GetNextVehicleNumber();
            return $"{prefix}{nextNumber}";
        }

        private async Task<int> GetNextVehicleNumber()
        {
            var maxCode = await _repository.GetMaxVehicleCodeAsync();

            if (string.IsNullOrEmpty(maxCode))
            {
                return 1000; 
            }

            if (int.TryParse(maxCode.Substring(1), out int currentNumber))
            {
                return currentNumber + 1;
            }

            return 1000; // Fallback if parsing fails
        }
    }
}
