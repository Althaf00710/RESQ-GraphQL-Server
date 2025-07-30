using Application.Services.Generic;
using Application.Utils;
using AutoMapper;
using Core.DTO;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class CivilianStatusService : Service<CivilianStatus>, ICivilianStatusService
    {
        private readonly ICivilianStatusRepository _repository;
        private readonly ILogger<CivilianStatusService> _logger;

        public CivilianStatusService(ICivilianStatusRepository repository, ILogger<CivilianStatusService> logger) : base(repository)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<CivilianStatus> Add(CivilianStatusCreateInput dto)
        {
            await ValidateAsync(dto.Role);

            var entity = new CivilianStatus
            {
                Role = dto.Role,
                Description = dto.Description
            };

            await _repository.AddAsync(entity);
            await _repository.SaveAsync();

            _logger.LogInformation("Civilian status with role {Role} created successfully", dto.Role);
            return entity;
        }


        public async Task<CivilianStatus> Update(int id, CivilianStatusUpdateInput dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
            {
                _logger.LogWarning("Civilian status with ID {Id} not found", id);
                throw new KeyNotFoundException($"Civilian status with ID {id} not found");
            }

            await ValidateAsync(dto.Role, excludeId: id);

            try
            {
                existing.Role = dto.Role;
                existing.Description = dto.Description;
                await _repository.SaveAsync();

                _logger.LogInformation("Civilian status with ID {Id} updated successfully", id);
                return existing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating civilian status with ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> RoleExistAsync(string role, int? excludeId = null)
        {
            return await _repository.RoleExistsAsync(role, excludeId);
        }

        private async Task ValidateAsync(string? role, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                _logger.LogWarning("Attempted to use a null or empty role");
                throw new ArgumentNullException(nameof(role), "Role cannot be null or empty");
            }

            if (await RoleExistAsync(role, excludeId))
            {
                _logger.LogWarning("Role {Role} already exists", role);
                throw new Exception("Another civilian status with this role already exists");
            }
        }





    }
}
