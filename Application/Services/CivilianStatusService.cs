using Application.Services.Generic;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace Application.Services
{
    public class CivilianStatusService : Service<CivilianStatus>, ICivilianStatusService
    {
        private readonly ICivilianStatusRepository _repository;

        public CivilianStatusService(ICivilianStatusRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
