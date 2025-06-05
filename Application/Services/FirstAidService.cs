using Application.Services.Generic;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace Application.Services
{
    public class FirstAidService : Service<FirstAid>, IFirstAidService
    {
        private readonly IFirstAidRepository _repository;
        public FirstAidService(IFirstAidRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
