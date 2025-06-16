using Application.Services.Generic;
using AutoMapper;
using Core.DTO;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class CivilianLocationService : Service<CivilianLocation>, ICivilianLocationService
    {
        private readonly ICivilianLocationRepository _repository;
        private readonly ILogger<CivilianLocationService> _logger;
        private readonly IMapper _mapper;

        public CivilianLocationService(ICivilianLocationRepository repository, ILogger<CivilianLocationService> logger, IMapper mapper) : base(repository)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<CivilianLocation> Handle(CivilianLocationInput dto)
        {
            try
            {
                if (dto == null)
                {
                    _logger.LogWarning("Received null DTO for civilian {CivilianId}", dto.CivilianId);
                    throw new ArgumentNullException(nameof(dto), "DTO cannot be null");
                }
         
                var existing = await _repository.GetByCivilianId(dto.CivilianId);

                if (existing != null)
                    return await Update(dto.CivilianId, dto);
                return await Add(dto); 
                //summa oru try, Let's see if it's working or not
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding or updating location for civilian {CivilianId}", dto.CivilianId);
                throw;
            }
        }

        private async Task<CivilianLocation> Add(CivilianLocationInput dto)
        {
            try
            {
                var entity = _mapper.Map<CivilianLocation>(dto);
                entity.Active = true; 

                await _repository.AddAsync(entity);
                await _repository.SaveAsync();

                _logger.LogInformation("Location created for civilian {CivilianId}", dto.CivilianId);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating civilian location");
                throw;
            }
        }

        private async Task<CivilianLocation> Update(int civilianId, CivilianLocationInput dto)
        {
            try
            {
                var existing = await _repository.GetByCivilianId(civilianId);   

                _mapper.Map(dto, existing);
                await _repository.SaveAsync();

                _logger.LogInformation("Location updated for civilian {CivilianId}", civilianId);
                return existing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating location for civilian {CivilianId}", civilianId);
                throw;
            }
        }

        //private async Task<bool> CheckCivilianId(int civilianId)
        //{
        //    try
        //    {
        //        var exists = await _repository.CheckCivilianId(civilianId);
        //        _logger.LogDebug("Civilian ID {CivilianId} check result: {Exists}", civilianId, exists);
        //        return exists;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error checking civilian ID {CivilianId}", civilianId);
        //        throw;
        //    }
        //}

        public async Task<CivilianLocation> GetByCivilianId(int civilianId)
        {
            try
            {
                var location = await _repository.GetByCivilianId(civilianId);
                if (location == null)
                {
                    _logger.LogWarning("No location found for civilian {CivilianId}", civilianId);
                    throw new Exception($"Location for civilian {civilianId} not found");
                }
                return location;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting location for civilian {CivilianId}", civilianId);
                throw;
            }
        }
    }
}
