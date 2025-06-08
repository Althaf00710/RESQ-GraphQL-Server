using Application.Services.Generic;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace Application.Services
{
    public class CivilianStatusRequestService : Service<CivilianStatusRequest>, ICivilianStatusRequestService
    {
        private readonly ICivilianStatusRequestRepository _repository;
        public CivilianStatusRequestService(ICivilianStatusRequestRepository repository)
            : base(repository)
        {
            _repository = repository;
        }
       
    }
}
