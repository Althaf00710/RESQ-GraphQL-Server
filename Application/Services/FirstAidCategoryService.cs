using Application.Services.Generic;
using Core.models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace Application.Services
{
    public class FirstAidCategoryService : Service<FirstAidCategory>, IFirstAidCategoryService
    {
        private readonly IFirstAidCategoryRepository _repository;
        public FirstAidCategoryService(IFirstAidCategoryRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
