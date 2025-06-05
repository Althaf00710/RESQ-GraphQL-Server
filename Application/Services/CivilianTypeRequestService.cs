using Application.Services.Generic;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace Application.Services
{
    public class CivilianTypeRequestService : Service<CivilianTypeRequest>, ICivilianTypeRequestService
    {
        private readonly ICivilianTypeRequestRepository _repository;
        public CivilianTypeRequestService(ICivilianTypeRequestRepository repository)
            : base(repository)
        {
            _repository = repository;
        }
       
    }
}
