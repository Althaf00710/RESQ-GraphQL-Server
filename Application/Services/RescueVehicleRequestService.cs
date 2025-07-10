using Application.Services.Generic;
using AutoMapper;
using Core.DTO;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class RescueVehicleRequestService : Service<RescueVehicleRequest>, IRescueVehicleRequestService
    {
        private readonly IRescueVehicleRequestRepository _repository;
        private readonly ILogger<RescueVehicleRequestService> _logger;
        private readonly IMapper _mapper;

        public RescueVehicleRequestService(IRescueVehicleRequestRepository repository, ILogger<RescueVehicleRequestService> logger, IMapper mapper) : base(repository)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<RescueVehicleRequest> Add(RescueVehicleRequestCreateInput dto)
        {
            _logger.LogInformation("Attempting to create new request", dto.EmergencyCategoryId);

            try
            {
                if (dto == null)
                {
                    _logger.LogWarning("Create request failed - null input received");
                    throw new ArgumentNullException(nameof(dto), "Request data cannot be null");
                }

                var hasRecentRequest = await _repository.CheckRecentReportings(dto.Longitude, dto.Latitude, dto.EmergencyCategoryId);

                if (hasRecentRequest)
                {
                    throw new InvalidOperationException("Recent request already exists in this area");
                }

                var request = _mapper.Map<RescueVehicleRequest>(dto);

                await _repository.AddAsync(request);
                await _repository.SaveAsync();

                _logger.LogInformation("Successfully created new rescue request ID {RequestId} (Status: {Status})", request.Id, request.Status);

                return request;
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping failed for rescue vehicle request creation");
                throw new ApplicationException("Invalid request data format", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating rescue vehicle request");
                throw;
            }
        }

        public async Task<RescueVehicleRequest> Update(int id, RescueVehicleRequestUpdateInput dto)
        {
            _logger.LogInformation("Attempting to update status for rescue request ID {RequestId} to {Status}", id, dto.Status);

            try
            {
                if (dto == null)
                {
                    _logger.LogWarning("Update failed - null input received");
                    throw new ArgumentNullException(nameof(dto), "Update data cannot be null");
                }

                var request = await _repository.GetByIdAsync(id);
                if (request == null)
                {
                    _logger.LogWarning("Update failed - request ID {RequestId} not found", id);
                    throw new KeyNotFoundException($"Rescue vehicle request with ID {id} not found");
                }

                // Validate status transition
                //if (!IsValidStatusTransition(request.Status, dto.Status))
                //{
                //    _logger.LogWarning("Invalid status transition from {OldStatus} to {NewStatus}",
                //        request.Status, dto.Status);
                //    throw new InvalidOperationException(
                //        $"Cannot change status from {request.Status} to {dto.Status}");
                //}

                _mapper.Map(dto, request);
                await _repository.SaveAsync();

                _logger.LogInformation("Successfully updated request ID {RequestId} to status {Status}", id, request.Status);

                return request;
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping failed during request update");
                throw new ApplicationException("Invalid status data format", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating request ID {RequestId}", id);
                throw;
            }
        }

        //private bool IsValidStatusTransition(string currentStatus, string newStatus)
        //{
        //    var validTransitions = new Dictionary<string, List<string>>
        //    {
        //        ["Searching"] = new List<string> { "Dispatched", "Cancelled" },
        //        ["Dispatched"] = new List<string> { "InProgress", "Cancelled" },
        //        ["InProgress"] = new List<string> { "Completed", "Failed" },
        //        ["Completed"] = new List<string>(),
        //        ["Failed"] = new List<string>(),
        //        ["Cancelled"] = new List<string>()
        //    };

        //    return validTransitions.TryGetValue(currentStatus, out var allowed) &&
        //           allowed.Contains(newStatus);
        //}
    }
}
