using System.Security.Cryptography;
using Application.Services.Generic;
using Application.Utils;
using AutoMapper;
using Core.DTO;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class CivilianService : Service<Civilian>, ICivilianService
    {
        private readonly ICivilianRepository _repository;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        private readonly JwtTokenGenerator _jwt;
        private readonly SmsSender _smsSender;
        private readonly EmailSender _emailSender;
        private readonly IMemoryCache _cache;
        public CivilianService(ICivilianRepository repository, ILogger<UserService> logger, IMapper mapper, JwtTokenGenerator jwt,
            SmsSender smsSender, EmailSender emailSender, IMemoryCache cache) : base(repository)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _jwt = jwt;
            _smsSender = smsSender;
            _emailSender = emailSender;
            _cache = cache;
        }

        public async Task<Civilian> Add(CivilianCreateInput dto)
        {
            _logger.LogInformation("Attempting to add new civilian with email: {@Dto}", dto);

            if (await _repository.GetByEmailAsync(dto.Email) is not null)
            {
                _logger.LogWarning("Civilian with email {Email} already exists", dto.Email);
                throw new Exception("Civilian Email already exists");
            }

            if (await _repository.GetByNICAsync(dto.NicNumber) is not null)
            {
                _logger.LogWarning("Civilian with NIC {NIC} already exists", dto.NicNumber);
                throw new Exception("NIC already exists");
            }

            if (await _repository.GetByPhoneNumberAsync(dto.PhoneNumber) is not null)
            {
                _logger.LogWarning("Civilian with phone number {Phone} already exists", dto.PhoneNumber);
                throw new Exception("Phone number already exists");
            }

            var civilian = _mapper.Map<Civilian>(dto);

            try
            {
                civilian.JoinedDate = DateTime.UtcNow;
                civilian.CivilianStatusId = 1;
                await _repository.AddAsync(civilian);
                await _repository.SaveAsync();
                await _smsSender.SendSmsAsync(civilian.PhoneNumber, "Welcome to ResQ! You’re now part of our emergency response network. We’re here 24/7. Stay safe!");
                await _emailSender.SendEmailAsync(civilian.Email, "Welcome to ResQ!", "You’re now part of our emergency response network. We’re here 24/7. Stay safe!");
                _logger.LogInformation("Civilian created successfully: {Email}", civilian.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating civilian with email: {Email}", dto.Email);
                throw;
            }
            return civilian;
        }

        public async Task<CivilianLogin> Login(string phoneNumber, int otp)
        {
            var civilian = await _repository.GetByPhoneNumberAsync(phoneNumber);
            if (civilian is null) throw new Exception("Account not found");

            if (!_cache.TryGetValue<int>(OtpKey(phoneNumber), out var code))
                throw new Exception("OTP expired or not requested");

            if (code != otp) throw new Exception("Invalid OTP");

            _cache.Remove(OtpKey(phoneNumber)); // one-time use

            return new CivilianLogin
            {
                JwtToken = _jwt.GenerateToken(civilian.Id.ToString(), civilian.PhoneNumber, "Civilian"),
                Civilian = civilian
            };
        }

        private static string OtpKey(string phone) => $"otp:civilian:{phone}";

        public async Task<bool> SendOTP(string phoneNumber)
        {
            var civilian = await _repository.GetByPhoneNumberAsync(phoneNumber);
            if (civilian is null) throw new Exception("Account not found");

            var code = RandomNumberGenerator.GetInt32(1000, 10000);
            _cache.Set(OtpKey(phoneNumber), code, TimeSpan.FromMinutes(5)); // expires in 5 min

            await _smsSender.SendSmsAsync(phoneNumber, $"ResQ code: {code} (valid 5 min)");
            if (!string.IsNullOrWhiteSpace(civilian.Email))
                await _emailSender.SendEmailAsync(civilian.Email, "Your ResQ code", $"Code: {code} (valid 5 min)");

            return true;
        }

        public async Task<Civilian> Update(int id, CivilianUpdateInput dto)
        {
            _logger.LogInformation("Attempting to update civilian: {@Dto}", dto);

            var civilian = await _repository.GetByIdAsync(id);
            if (civilian == null)
            {
                _logger.LogWarning("Civilian with ID {Id} not found", id);
                throw new Exception("Civilian not found");
            }

            if (dto.Email != null && dto.Email != civilian.Email)
            {
                if (await _repository.GetByEmailAsync(dto.Email) is not null)
                {
                    _logger.LogWarning("Civilian with email {Email} already exists", dto.Email);
                    throw new Exception("Civilian email already exists");
                }
                civilian.Email = dto.Email;
            }

            if (dto.NicNumber != null && dto.NicNumber != civilian.NicNumber)
            {
                if (await _repository.GetByNICAsync(dto.NicNumber) is not null)
                {
                    _logger.LogWarning("Civilian with NIC {NIC} already exists", dto.NicNumber);
                    throw new Exception("NIC already exists");
                }
                civilian.NicNumber = dto.NicNumber;
            }

            if (dto.PhoneNumber != null && dto.PhoneNumber != civilian.PhoneNumber)
            {
                if (await _repository.GetByPhoneNumberAsync(dto.PhoneNumber) is not null)
                {
                    _logger.LogWarning("Civilian with phone number {Phone} already exists", dto.PhoneNumber);
                    throw new Exception("Phone number already exists");
                }
                civilian.PhoneNumber = dto.PhoneNumber;
            }

            if (dto.Name != null) civilian.Name = dto.Name;

            try
            {
                await _repository.SaveAsync();
                _logger.LogInformation("Civilian updated successfully: {@Civilian}", civilian);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating civilian: {@Dto}", dto);
                throw;
            }

            return civilian;
        }

        public async Task<bool> updateCivilianStatus(int id, int civilianStatusId)
        {
            var civilian = await _repository.GetByIdAsync(id);
            if (civilian == null)
            {
                _logger.LogWarning("Civilian with ID {Id} not found", id);
                return false;
            }

            civilian.CivilianStatusId = civilianStatusId;
            _repository.SaveAsync();
            return true;
        }

        public async Task<bool> Restrict(int id)
        {
            var civilian = await _repository.GetByIdAsync(id);
            if (civilian == null)
            {
                _logger.LogWarning("Civilian with ID {Id} not found", id);
                return false;
            }
            civilian.IsRestrict = true;
            await _repository.SaveAsync();
            return true;
        }

        public async Task<bool> Unrestrict(int id)
        {
            var civilian = await _repository.GetByIdAsync(id);
            if (civilian == null)
            {
                _logger.LogWarning("Civilian with ID {Id} not found", id);
                return false;
            }
            civilian.IsRestrict = false;
            await _repository.SaveAsync();
            return true;
        }

        public IQueryable<Civilian> Query() =>
            _repository.Query();

    }
}
