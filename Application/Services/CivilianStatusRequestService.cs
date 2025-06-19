using Application.Services.Generic;
using Application.Utils;
using AutoMapper;
using Core.DTO;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class CivilianStatusRequestService : Service<CivilianStatusRequest>, ICivilianStatusRequestService
    {
        private readonly ICivilianStatusRequestRepository _repository;
        private readonly ILogger<CivilianStatusRequestService> _logger;
        private readonly IMapper _mapper;
        private readonly ICivilianService _civilianService;

        public CivilianStatusRequestService(ICivilianStatusRequestRepository repository, 
            ILogger<CivilianStatusRequestService> logger, 
            IMapper mapper, 
            JwtTokenGenerator jwt, 
            ICivilianService civilianService)
            : base(repository)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _civilianService = civilianService;
        }

        public async Task<CivilianStatusRequest> Add(CivilianStatusRequestCreateInput dto, IFile? proofPicture)
        {
            _logger.LogInformation("Attempting to add CivilianStatusRequest for CivilianId: {CivilianId}", dto.CivilianId);

            if (dto.CivilianId <= 0)
            {
                _logger.LogWarning("Invalid CivilianId: {CivilianId}", dto.CivilianId);
                throw new Exception("Civilian Id is required.");
            }

            if (dto.CivilianStatusId <= 0)
            {
                _logger.LogWarning("Invalid CivilianStatusId: {CivilianStatusId}", dto.CivilianStatusId);
                throw new Exception("Civilian Status Id is required.");
            }

            if (proofPicture is null)
            {
                _logger.LogWarning("Proof image not provided for CivilianStatusRequest");
                throw new Exception("Proof image is required.");
            }

            var request = _mapper.Map<CivilianStatusRequest>(dto);

            try
            {
                request.proofImage = await FileHandler.StoreImage("civilian-status-requests", proofPicture);

                await _repository.AddAsync(request);
                await _repository.SaveAsync();

                _logger.LogInformation("Civilian Status Requested successfully for Civilian Id: {CivilianId}", dto.CivilianId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating CivilianStatusRequest for CivilianId: {CivilianId}", dto.CivilianId);
                throw;
            }

            return request;
        }

        public async Task<CivilianStatusRequest> Update(int id, CivilianStatusRequestUpdateInput dto)
        {
            _logger.LogInformation("Attempting to update CivilianStatusRequest ID: {Id}", id);

            var request = await _repository.GetByIdAsync(id);
            if (request is null)
            {
                _logger.LogWarning("CivilianStatusRequest not found with ID: {Id}", id);
                throw new Exception("Civilian status request not found.");
            }

            var validStatuses = new[] { "Approved", "Rejected" };
            if (!validStatuses.Contains(dto.Status))
            {
                _logger.LogWarning("Invalid status value: {Status} for CivilianStatusRequest ID: {Id}", dto.Status, id);
                throw new Exception("Invalid status. Allowed values: Approved, Rejected.");
            }

            request.status = dto.Status;
            request.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _repository.Update(request);
                await _repository.SaveAsync();

                _civilianService.updateCivilianStatus(request.CivilianId, request.CivilianStatusId);
                _logger.LogInformation("CivilianStatusRequest ID: {Id} updated successfully", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating CivilianStatusRequest ID: {Id}", id);
                throw;
            }

            return request;
        }

    }
}
