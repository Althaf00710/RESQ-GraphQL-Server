using Application.Services.Generic;
using Application.Utils;
using Core.DTO;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class RescueVehicleCategoryService : Service<RescueVehicleCategory>, IRescueVehicleCategoryService
    {
        private readonly IRescueVehicleCategoryRepository _repository;
        private readonly ILogger<RescueVehicleCategoryService> _logger;

        public RescueVehicleCategoryService(
            IRescueVehicleCategoryRepository repository,
            ILogger<RescueVehicleCategoryService> logger,
            JwtTokenGenerator jwt) : base(repository)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<RescueVehicleCategory> Add(RescueVehicleCategoryCreateInput dto)
        {
            await ValidateAsync(dto.Name);

            var entity = new RescueVehicleCategory
            {
                Name = dto.Name
            };

            await _repository.AddAsync(entity);
            await _repository.SaveAsync();

            _logger.LogInformation("Rescue vehicle category '{CategoryName}' created successfully", dto.Name);
            return entity;
        }

        public async Task<RescueVehicleCategory> Update(int id, RescueVehicleCategoryUpdateInput dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
            {
                _logger.LogWarning("Rescue vehicle category with ID {CategoryId} not found", id);
                throw new KeyNotFoundException($"Rescue vehicle category with ID {id} not found");
            }

            await ValidateAsync(dto.Name, excludeId: id);

            try
            {
                existing.Name = dto.Name;
                await _repository.SaveAsync();

                _logger.LogInformation(
                    "Rescue vehicle category with ID {CategoryId} updated to '{CategoryName}'",
                    id, dto.Name);
                return existing;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error updating rescue vehicle category with ID {CategoryId}",
                    id);
                throw;
            }
        }

        private async Task ValidateAsync(string? category, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                _logger.LogWarning("Attempted to use a null or empty category name");
                throw new ArgumentNullException(nameof(category), "Category name cannot be null or empty");
            }

            if (await _repository.CategoryExistsAsync(category, excludeId))
            {
                _logger.LogWarning("Category name '{CategoryName}' already exists", category);
                throw new InvalidOperationException("Another rescue vehicle category with this name already exists");
            }
        }
    }
}