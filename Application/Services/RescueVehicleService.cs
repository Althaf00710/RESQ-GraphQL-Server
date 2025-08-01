using Application.Services.Generic;
using Application.Utils;
using AutoMapper;
using Core.DTO;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using HotChocolate.Subscriptions;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class RescueVehicleService : Service<RescueVehicle>, IRescueVehicleService
    {
        private readonly IRescueVehicleRepository _repository;
        private readonly IRescueVehicleCategoryRepository _categoryRepository;
        private readonly ILogger<RescueVehicleService> _logger;
        private readonly IMapper _mapper;
        private readonly JwtTokenGenerator _jwt;
        private readonly ITopicEventSender _topicEventSender;

        public RescueVehicleService(IRescueVehicleRepository repository, IRescueVehicleCategoryRepository categoryRepository, 
            ILogger<RescueVehicleService> logger, IMapper mapper, JwtTokenGenerator jwt, ITopicEventSender topicEventSender) : base(repository)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _logger = logger;
            _mapper = mapper;
            _jwt = jwt;
            _topicEventSender = topicEventSender;
        }

        public async Task<RescueVehicle> Add(RescueVehicleCreateInput dto)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(dto.RescueVehicleCategoryId);
                if (category == null)
                {
                    _logger.LogWarning("Rescue vehicle category with ID {CategoryId} not found", dto.RescueVehicleCategoryId);
                    throw new Exception("Rescue vehicle category not found");
                }

                var vehicle = _mapper.Map<RescueVehicle>(dto);
                vehicle.Code = await GenerateVehicleCode(category);
                vehicle.Password = PasswordHash.HashPassword(dto.Password);

                await _repository.AddAsync(vehicle);
                await _repository.SaveAsync();

                _logger.LogInformation("Rescue vehicle created successfully with code {VehicleCode}", vehicle.Code);

                return vehicle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating rescue vehicle");
                throw;
            }
        }

        public async Task<RescueVehicle> Update(int id, RescueVehicleUpdateInput dto)
        {
            try
            {
                var vehicle = await _repository.GetByIdAsync(id);
                if (vehicle == null)
                {
                    throw new Exception($"Rescue vehicle with ID {id} not found");
                }

                _mapper.Map(dto, vehicle);
                await _repository.SaveAsync();

                _logger.LogInformation("Rescue vehicle {VehicleCode} updated successfully", vehicle.Code);
                return vehicle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating rescue vehicle with ID {VehicleId}", id);
                throw;
            }
        }

        private async Task<string> GenerateVehicleCode(RescueVehicleCategory category)
        {
            string prefix = category.Name[0].ToString().ToUpper();
            int nextNumber = await GetNextVehicleNumber();
            return $"{prefix}{nextNumber}";
        }

        private async Task<int> GetNextVehicleNumber()
        {
            var maxCode = await _repository.GetMaxVehicleCodeAsync();

            if (string.IsNullOrEmpty(maxCode))
            {
                return 1000; 
            }

            if (int.TryParse(maxCode.Substring(1), out int currentNumber))
            {
                return currentNumber + 1;
            }

            return 1000; // Fallback if parsing fails
        }

        public IQueryable<RescueVehicle> Query() =>
            _repository.Query();

        public async Task<RescueVehicleLogin> Login(string plateNumber, string password)
        {
            if (string.IsNullOrWhiteSpace(plateNumber) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Plate number and password must not be empty.");
            }

            var vehicle = await _repository.GetByPlateNumberAsync(plateNumber);

            if (vehicle is null)
            {
                _logger.LogWarning("Login failed: Rescue vehicle with plate number '{PlateNumber}' not found", plateNumber);
                throw new Exception("Invalid plate number or password.");
            }

            var isPasswordValid = PasswordHash.VerifyPassword(password, vehicle.Password);

            if (!isPasswordValid)
            {
                _logger.LogWarning("Login failed: Invalid password for plate number '{PlateNumber}'", plateNumber);
                throw new Exception("Invalid plate number or password.");
            }

            vehicle.Status = "Active";
            await _repository.SaveAsync();

            await EventVehicleStatusAsync(vehicle);

            _logger.LogInformation("Rescue vehicle '{PlateNumber}' token generated successfully", plateNumber);

            return new RescueVehicleLogin
            {
                JwtToken = _jwt.GenerateToken(vehicle.Id.ToString(), vehicle.PlateNumber, "RescueVehicle"),
                RescueVehicle = vehicle
            };
        }

        private async Task EventVehicleStatusAsync(RescueVehicle vehicle)
        {
            await _topicEventSender.SendAsync("VehicleStatusChanged", vehicle);
        }
    }
}
