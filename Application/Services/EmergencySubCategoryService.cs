using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class EmergencySubCategoryService : Service<EmergencySubCategory>, IEmergencySubCategoryService
    {
        private readonly IEmergencySubCategoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EmergencySubCategoryService> _logger;
        public EmergencySubCategoryService(IEmergencySubCategoryRepository repository, IMapper mapper, ILogger<EmergencySubCategoryService> logger) : base(repository)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<EmergencySubCategory> Add(EmergencySubCategoryCreateInput dto, IFile? image)
        {
            await Validate(dto.Name, dto.EmergencyCategoryId);

            var entity = _mapper.Map<EmergencySubCategory>(dto);
            await _repository.AddAsync(entity);
            await _repository.SaveAsync();

            _logger.LogInformation("Emergency subcategory {SubCategoryName} created", dto.Name);
            return entity;
        }

        public async Task<List<EmergencySubCategory>> GetByCategoryIdAsync(int categoryId)
        {
            return await _repository.GetByCategoryIdAsync(categoryId);
        }

        public async Task<EmergencySubCategory> Update(int id, EmergencySubCategoryUpdateInput dto, IFile? image)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Emergency subcategory with ID {SubCategoryId} not found", id);
                throw new Exception($"Emergency subcategory with ID {id} not found");
            }

            if (dto.Name != null && dto.Name != existing.Name)
            {
                await Validate(dto.Name, existing.EmergencyCategoryId, id);
            }

            _mapper.Map(dto, existing);
            await _repository.SaveAsync();

            _logger.LogInformation("Emergency subcategory {SubCategoryId} updated", id);
            return existing;
        }

        public async Task<bool> CheckExist(string name, int categoryId, int? excludeId=null)
        {
            return await _repository.ExistAsync(name, categoryId, excludeId);
        } 

        private async Task Validate(string name, int categoryId, int? excludeId = null)
        {
            if (await CheckExist(name, categoryId, excludeId))
            {
                var message = $"Subcategory '{name}' already exists in this category";
                _logger.LogWarning(message);
                throw new Exception(message);
            }
        }
    }
}
