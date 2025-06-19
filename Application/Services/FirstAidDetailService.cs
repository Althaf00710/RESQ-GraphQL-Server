using Application.Services.Generic;
using AutoMapper;
using Core.DTO;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class FirstAidDetailService : Service<FirstAidDetail>, IFirstAidDetailService
    {
        private readonly IFirstAidDetailRepository _repository;
        private readonly ILogger<FirstAidDetailService> _logger;
        private readonly IMapper _mapper;
        public FirstAidDetailService(IFirstAidDetailRepository repository, ILogger<FirstAidDetailService> logger, IMapper mapper) : base(repository)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FirstAidDetail> Add(FirstAidDetailCreateInput dto, IFile? image)
        {
            await ValidateFirstAidDetail(dto.EmergencySubCategoryId, dto.Point);

            var entity = _mapper.Map<FirstAidDetail>(dto);
            await _repository.AddAsync(entity);
            await _repository.SaveAsync();

            _logger.LogInformation("First aid detail created for subcategory {SubCategoryId}", dto.EmergencySubCategoryId);
            return entity;
        }

        public async Task<IEnumerable<FirstAidDetail>> GetBySubCategoryIdAsync(int emergencySubCategoryId)
        {
            return await _repository.GetBySubCategoryIdAsync(emergencySubCategoryId);
        }

        public async Task<FirstAidDetail> Update(int id, FirstAidDetailUpdateInput dto, IFile? image)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("First aid detail with ID {DetailId} not found", id);
                throw new KeyNotFoundException($"First aid detail with ID {id} not found");
            }

            if (dto.Point != null && dto.Point != existing.Point)
            {
                await ValidateFirstAidDetail(existing.EmergencySubCategoryId, dto.Point, id);
            }

            _mapper.Map(dto, existing);
            await _repository.SaveAsync();

            _logger.LogInformation("First aid detail {DetailId} updated", id);
            return existing;
        }

        private async Task ValidateFirstAidDetail(int emergencySubCategoryId, string point, int? excludeId = null)
        {
            if (await _repository.ExistsAsync(emergencySubCategoryId, point, excludeId))
            {
                var message = $"First aid point '{point}' already exists in this subcategory";
                _logger.LogWarning(message);
                throw new InvalidOperationException(message);
            }
        }
    }
}
