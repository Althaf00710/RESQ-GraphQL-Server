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
        public Task<EmergencySubCategory> Add(EmergencySubCategoryCreateInput dto, IFile? image)
        {
            throw new NotImplementedException();
        }

        public Task<EmergencySubCategory> Update(int id, EmergencySubCategoryUpdateInput dto, IFile? image)
        {
            throw new NotImplementedException();
        }
    }
}
