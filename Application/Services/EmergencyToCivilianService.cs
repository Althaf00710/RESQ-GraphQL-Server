using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Generic;
using AutoMapper;
using Core.DTO;
using Core.Models;
using Core.Repositories.Generic;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class EmergencyToCivilianService : Service<EmergencyToCivilian>, IEmergencyToCivilianService
    {
        private readonly IEmergencyToCivilianRepository _repository;
        private readonly ILogger<EmergencyToCivilianService> _logger;
        private readonly IMapper _mapper;

        public EmergencyToCivilianService(IEmergencyToCivilianRepository repository, ILogger<EmergencyToCivilianService> logger, IMapper mapper) : base(repository)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<EmergencyToCivilian> Add(EmergencyToCivilianInput input)
        {
            if (await _repository.ExistsAsync(input.EmergencyCategoryId, input.CivilianStatusId))
            {
                _logger.LogWarning("Mapping already exists between emergency category {CategoryId} and civilian status {StatusId}",
                    input.EmergencyCategoryId, input.CivilianStatusId);
                throw new InvalidOperationException("This mapping already exists");
            }

            var entity = _mapper.Map<EmergencyToCivilian>(input);
            await _repository.AddAsync(entity);
            await _repository.SaveAsync();

            _logger.LogInformation("Created new emergency-to-civilian mapping: {MappingId}", entity.Id);
            return entity;
        }

        public async Task<IEnumerable<EmergencyToCivilian>> GetByCivilianStatusAsync(int civilianStatusId)
        {
            return await _repository.GetByCivilianStatusAsync(civilianStatusId);
        }

        public async Task<IEnumerable<EmergencyToCivilian>> GetByEmergencyCategoryAsync(int emergencyCategoryId)
        {
            return await _repository.GetByEmergencyCategoryAsync(emergencyCategoryId);
        }
    }
}
