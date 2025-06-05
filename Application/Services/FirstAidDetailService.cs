using Application.Services.Generic;
using Core.models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace Application.Services
{
    public class FirstAidDetailService : Service<FirstAidDetail>, IFirstAidDetailService
    {
        private readonly IFirstAidDetailRepository _repository;
        public FirstAidDetailService(IFirstAidDetailRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
