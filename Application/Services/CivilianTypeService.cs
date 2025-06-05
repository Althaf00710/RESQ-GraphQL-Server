using Application.Services.Generic;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace Application.Services
{
    public class CivilianTypeService : Service<CivilianType>, ICivilianTypeService
    {
        private readonly ICivilianTypeRepository _repository;

        public CivilianTypeService(ICivilianTypeRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
