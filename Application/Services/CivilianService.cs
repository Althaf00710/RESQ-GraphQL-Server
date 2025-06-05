using Application.Services.Generic;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace Application.Services
{
    public class CivilianService : Service<Civilian>, ICivilianService
    {
        private readonly ICivilianRepository _repository;

        public CivilianService(ICivilianRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
