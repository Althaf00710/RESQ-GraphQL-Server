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
using Microsoft.EntityFrameworkCore;

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
        private readonly ICivilianLocationRepository _civilianLocationRepository;

        public RescueVehicleRequestService(IRescueVehicleRequestRepository repository, ILogger<RescueVehicleRequestService> logger,
            IMapper mapper, ICivilianService civilianService, ITopicEventSender topicEventSender, IRescueVehicleLocationRepository vehicleLocationRepository,
            IAssignmentQueue assignmentQueue, ICivilianLocationRepository civilianLocationRepository) : base(repository)
        {
            _repository = repository;
            _vehicleLocationRepository = vehicleLocationRepository;
            _logger = logger;
            _mapper = mapper;
            _civilianService = civilianService;
            _topicEventSender = topicEventSender;
            _assignmentQueue = assignmentQueue;
            _civilianLocationRepository = civilianLocationRepository;
        }

        public async Task<RescueVehicleRequest> Add(RescueVehicleRequestCreateInput dto, IFile? image)
        {
            _logger.LogInformation("Attempting to create new request for subcategory {SubCatId}", dto.EmergencySubCategoryId);

            try
            {
                if (dto == null)
                {
                    _logger.LogWarning("Create request failed - null input received");
                    throw new ArgumentNullException(nameof(dto), "Request data cannot be null");
                }

                var hasRecentRequest = await _repository.CheckRecentReportings(
                    dto.Longitude, dto.Latitude, dto.EmergencySubCategoryId);

                if (hasRecentRequest)
                    throw new InvalidOperationException("Recent request already exists in this area");

                var request = _mapper.Map<RescueVehicleRequest>(dto);

                if (image is not null)
                {
                    try
                    {
                        request.ProofImageURL = await FileHandler.StoreImage("emergency-request", image);
                        _logger.LogInformation("Image stored at: {Path}", request.ProofImageURL);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to store image");
                        throw new Exception("Failed to upload image");
                    }
                }

                await _repository.AddAsync(request);
                await _repository.SaveAsync();

                // Acknowledge reporter
                await _civilianService.NotifyCivilian(dto.CivilianId,
                    "Your Rescue Vehicle request has been received and is being processed.");

                // Nearest vehicles (existing logic)
                var nearestVehicleIds = await _vehicleLocationRepository.GetNearestVehicleIdsAsync(
                    dto.Latitude, dto.Longitude, dto.EmergencySubCategoryId, maxCount: 5);

                // Start the offer flow (15s per vehicle, stop on accept)
                _assignmentQueue.Enqueue(request, nearestVehicleIds);

                // ---------------------------
                // 🔔 Notify nearby civilians
                // ---------------------------

                // We need the *category* id for eligibility mapping (EmergencyToCivilians).
                var meta = await _repository.Query()
                    .Where(r => r.Id == request.Id)
                    .Select(r => new
                    {
                        CatId = r.EmergencySubCategory.EmergencyCategoryId,
                        SubName = r.EmergencySubCategory.Name
                    })
                    .FirstAsync();

                const double RADIUS_METERS = 1000000; // tune this radius
                var nearbyCivilianIds = await _civilianLocationRepository.GetNearbyCivilianIdsAsync(
                    dto.Latitude, dto.Longitude, meta.CatId, RADIUS_METERS);
                _logger.LogInformation("Found {Count} nearby civilians within {Radius}m for category {CatId}",
                    nearbyCivilianIds.Count, RADIUS_METERS, meta.CatId);
                // Publish one event per civilian on a unique topic
                foreach (var civId in nearbyCivilianIds)
                {
                    var payload = new NearbyEmergencyPayload
                    {
                        Emergency = request,   // includes address/status/coords/subcategory via your model
                        CivilianId = civId
                    };

                    await _topicEventSender.SendAsync($"CivilianNearby:{civId}", payload);
                    _logger.LogInformation("Notified civilian {CivilianId} of nearby emergency", civId);
                }

                _logger.LogInformation(
                    "Created rescue request {RequestId} (Status: {Status}); notified {Count} civilians within {Radius}m",
                    request.Id, request.Status, nearbyCivilianIds.Count, RADIUS_METERS);

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

                if (string.Equals(request.Status, "Cancelled", StringComparison.OrdinalIgnoreCase))
                {
                    _assignmentQueue.Cancel(request.Id);
                }

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
