using Application.Services.Generic;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace Application.Services
{
    public class CivilianLocationService : Service<CivilianLocation>, ICivilianLocationService
    {
        private readonly ICivilianLocationRepository _repository;

        public CivilianLocationService(ICivilianLocationRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
