using Application.Services.Generic;
using Application.Utils;
using AutoMapper;
using Core.DTO;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Concurrent;
using NetTopologySuite.Geometries;
using Application.Utils.AssignmentOffer;

namespace Application.Services
{
    public class RescueVehicleRequestService : Service<RescueVehicleRequest>, IRescueVehicleRequestService
    {
        private readonly IRescueVehicleRequestRepository _repository;
        private readonly IRescueVehicleLocationRepository _vehicleLocationRepository;
        private readonly ILogger<RescueVehicleRequestService> _logger;
        private readonly IMapper _mapper;
        private readonly ICivilianService _civilianService;
        private readonly ITopicEventSender _topicEventSender;
        private readonly IAssignmentQueue _assignmentQueue;

        public RescueVehicleRequestService(IRescueVehicleRequestRepository repository, ILogger<RescueVehicleRequestService> logger,
            IMapper mapper, ICivilianService civilianService, ITopicEventSender topicEventSender, IRescueVehicleLocationRepository vehicleLocationRepository,
            IAssignmentQueue assignmentQueue) : base(repository)
        {
            _repository = repository;
            _vehicleLocationRepository = vehicleLocationRepository;
            _logger = logger;
            _mapper = mapper;
            _civilianService = civilianService;
            _topicEventSender = topicEventSender;
            _assignmentQueue = assignmentQueue;
        }

        public async Task<RescueVehicleRequest> Add(RescueVehicleRequestCreateInput dto, IFile? image)
        {
            _logger.LogInformation("Attempting to create new request", dto.EmergencySubCategoryId);

            try
            {
                if (dto == null)
                {
                    _logger.LogWarning("Create request failed - null input received");
                    throw new ArgumentNullException(nameof(dto), "Request data cannot be null");
                }

                var hasRecentRequest = await _repository.CheckRecentReportings(dto.Longitude, dto.Latitude, dto.EmergencySubCategoryId);

                if (hasRecentRequest)
                {
                    throw new InvalidOperationException("Recent request already exists in this area");
                }

                var request = _mapper.Map<RescueVehicleRequest>(dto);

                if (image is not null)
                {
                    try
                    {
                        request.ProofImageURL = await FileHandler.StoreImage("emergency-request", image);
                        _logger.LogInformation("image stored at: {Path}", request.ProofImageURL);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to store image");
                        throw new Exception("Failed to upload mage");
                    }
                }

                await _repository.AddAsync(request);
                await _repository.SaveAsync();

                await _civilianService.NotifyCivilian(dto.CivilianId, "Your Rescue Vehicle request has been received and is being processed.");

                var nearestVehicleIds = await _vehicleLocationRepository
                    .GetNearestVehicleIdsAsync(dto.Latitude, dto.Longitude, maxCount: 5);

                // Start the offer flow (15s per vehicle, stop on accept)
                _assignmentQueue.Enqueue(request, nearestVehicleIds);

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

                _mapper.Map(dto, request);
                await _repository.SaveAsync();
                await EventRequestUpdateAsync(request);
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

        public IQueryable<RescueVehicleRequest> Query() =>
            _repository.Query();

        private async Task EventRequestUpdateAsync(RescueVehicleRequest request)
        {
            await _topicEventSender.SendAsync("VehicleRequestStatusChanged", request);
        }

    }
}
