using Application.Services.Generic;
using Application.Utils;
using AutoMapper;
using Core.DTO;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class CivilianService : Service<Civilian>, ICivilianService
    {
        private readonly ICivilianRepository _repository;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        private readonly JwtTokenGenerator _jwt;

        public CivilianService(ICivilianRepository repository, ILogger<UserService> logger, IMapper mapper, JwtTokenGenerator jwt) : base(repository)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _jwt = jwt;
        }

        public Task<Civilian> Add(CivilianCreateInput dto)
        {
            throw new NotImplementedException();
        }

        public Task<string> Login(string phoneNumber, int otp)
        {
            throw new NotImplementedException();
        }

        public Task<string> SendOTP(string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public Task<Civilian> Update(int id, CivilianCreateInput dto)
        {
            throw new NotImplementedException();
        }
    }
}
