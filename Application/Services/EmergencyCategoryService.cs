
using Application.Services.Generic;
using Application.Utils;
using AutoMapper;
using Core.DTO;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Generic;
using Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class EmergencyCategoryService : Service<EmergencyCategory>, IEmergencyCategoryService
    {
        private readonly IEmergencyCategoryRepository _repository;
        private readonly ILogger<EmergencyCategoryService> _logger;
        private readonly IMapper _mapper;

        public EmergencyCategoryService(IEmergencyCategoryRepository repository, ILogger<EmergencyCategoryService> logger, IMapper mapper) : base(repository)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<EmergencyCategory> Add(EmergencyCategoryCreateInput dto)
        {
            await ValidateAsync(dto.Name);

            var entity = _mapper.Map<EmergencyCategory>(dto);

            await _repository.AddAsync(entity);
            await _repository.SaveAsync();

            _logger.LogInformation("Emergency category '{Name}' created successfully", dto.Name);
            return entity;
        }

        public async Task<EmergencyCategory> Update(int id, EmergencyCategoryUpdateInput dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
            {
                _logger.LogWarning("Emergency category with ID {Id} not found", id);
                throw new KeyNotFoundException($"Emergency category with ID {id} not found");
            }

            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                await ValidateAsync(dto.Name, excludeId: id);
            }

            _mapper.Map(dto, existing);
            await _repository.SaveAsync();

            _logger.LogInformation("Emergency category with ID {Id} updated successfully", id);
            return existing;
        }

        private async Task ValidateAsync(string? name, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Attempted to use a null or empty category name");
                throw new ArgumentNullException(nameof(name), "Category name cannot be null or empty");
            }

            if (await _repository.ExistAsync(name, excludeId))
            {
                _logger.LogWarning("Category name '{Name}' already exists", name);
                throw new Exception("Another emergency category with this name already exists");
            }
        }
    }
}
