
using Application.Services.Generic;
using Application.Utils;
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

        public EmergencyCategoryService(IEmergencyCategoryRepository repository, ILogger<EmergencyCategoryService> logger) : base(repository)
        {
            _repository = repository;
            _logger = logger;
        }
        public Task<EmergencyCategory> Add(EmergencyCategoryCreateInput dto)
        {
            throw new NotImplementedException();
        }

        public Task<EmergencyCategory> Update(int id, EmergencyCategoryUpdateInput dto)
        {
            throw new NotImplementedException();
        }
    }
}
