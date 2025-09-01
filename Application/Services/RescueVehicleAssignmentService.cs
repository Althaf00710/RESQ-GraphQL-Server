using Application.Services.Generic;
using AutoMapper;
using Core.DTO;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class RescueVehicleAssignmentService : Service<RescueVehicleAssignment>, IRescueVehicleAssignmentService
    {
        private readonly IRescueVehicleAssignmentRepository _repository;
        private readonly IRescueVehicleRequestRepository _requestRepository;
        private readonly IRescueVehicleRepository _vehicleRepository;
        private readonly ILogger<RescueVehicleAssignmentService> _logger;
        private readonly IMapper _mapper;
        private readonly IRescueVehicleRequestService _rescueVehicleRequestService;

        public RescueVehicleAssignmentService(IRescueVehicleAssignmentRepository repository,
            IRescueVehicleRequestRepository requestRepository,
            IRescueVehicleRepository vehicleRepository,
            ILogger<RescueVehicleAssignmentService> logger, 
            IMapper mapper,
            IRescueVehicleRequestService rescueVehicleRequestService) : base(repository)
        {
            _repository = repository;
            _requestRepository = requestRepository;
            _vehicleRepository = vehicleRepository;
            _logger = logger;
            _mapper = mapper;
            _rescueVehicleRequestService = rescueVehicleRequestService;
        }

        public async Task<RescueVehicleAssignment> Add(AssignmentCreateInput dto)
        {
            _logger.LogInformation("Creating new assignment for request {RequestId} and vehicle {VehicleId}", dto.RescueVehicleRequestId, dto.RescueVehicleId);

            try
            {
                await ValidateAssignmentCreation(dto);

                var assignment = _mapper.Map<RescueVehicleAssignment>(dto);
                assignment.Timestamp = DateTime.Now;
                await _rescueVehicleRequestService.Update(dto.RescueVehicleRequestId, new RescueVehicleRequestUpdateInput { Status = "Dispatched" });

                await _repository.AddAsync(assignment);
                await _repository.SaveAsync();

                _logger.LogInformation("Created assignment ID {AssignmentId} for request {RequestId}", assignment.Id, assignment.RescueVehicleRequestId);

                return assignment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating assignment for request {RequestId}", dto.RescueVehicleRequestId);
                throw;
            }
        }

        public async Task<RescueVehicleAssignment> Update(int id, AssignmentUpdateInput dto)
        {
            _logger.LogInformation("Updating assignment {AssignmentId} with status {Status}", id, dto.Status);

            try
            {
                var assignment = await _repository.GetByIdAsync(id)
                    ?? throw new KeyNotFoundException("Assignment not found");

                // Load the owning request (status now lives here)
                var request = await _requestRepository.GetByIdAsync(assignment.RescueVehicleRequestId)
                    ?? throw new KeyNotFoundException($"Rescue vehicle request {assignment.RescueVehicleRequestId} not found");

                // Validate transition using the request's current status
                ValidateRequestStatusTransition(request.Status, dto.Status);

                // Timestamps that are tied to the assignment’s lifecycle
                var now = DateTime.Now;
                switch (dto.Status)
                { 
                    case "Arrived":
                        assignment.ArrivalTime ??= now;
                        break;

                    case "Completed":
                        assignment.DepartureTime ??= now;
                        break;

                    case "Cancelled":
                        assignment.ArrivalTime = null;
                        assignment.DepartureTime = null;
                        break;
                }

                // Persist request status change + broadcast subscription event
                await _rescueVehicleRequestService.Update(request.Id,
                    new RescueVehicleRequestUpdateInput { Status = dto.Status });

                await _repository.SaveAsync();

                _logger.LogInformation("Successfully updated assignment {AssignmentId}; request {RequestId} now {Status}", id, request.Id, dto.Status);
                return assignment;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Status transition validation failed for assignment {AssignmentId}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating assignment {AssignmentId}", id);
                throw;
            }
        }


        private void ValidateRequestStatusTransition(string currentRequestStatus, string newStatus)
        {
            var transitions = new Dictionary<string, List<string>>
            {
                ["Searching"] = new() { "Dispatched", "Cancelled" },
                ["Dispatched"] = new() { "Arrived", "Cancelled" },
                ["Arrived"] = new() { "Completed" },
                ["Completed"] = new(),
                ["Cancelled"] = new(),
            };

            if (!transitions.TryGetValue(currentRequestStatus ?? "", out var allowed) ||
                !allowed.Contains(newStatus))
            {
                throw new InvalidOperationException(
                    $"Invalid status transition from '{currentRequestStatus}' to '{newStatus}'. " +
                    $"Allowed: {(allowed?.Any() == true ? string.Join(", ", allowed) : "none")}");
            }
        }

        private async Task ValidateAssignmentCreation(AssignmentCreateInput dto)
        {
            // Validate request exists
            var request = await _requestRepository.GetByIdAsync(dto.RescueVehicleRequestId);
            if (request == null)
            {
                _logger.LogWarning("Request {RequestId} not found", dto.RescueVehicleRequestId);
                throw new KeyNotFoundException($"Rescue vehicle request {dto.RescueVehicleRequestId} not found");
            }

            // Validate vehicle exists
            var vehicle = await _vehicleRepository.GetByIdAsync(dto.RescueVehicleId);
            if (vehicle == null)
            {
                _logger.LogWarning("Vehicle {VehicleId} not found", dto.RescueVehicleId);
                throw new KeyNotFoundException($"Rescue vehicle {dto.RescueVehicleId} not found");
            }

            // Check vehicle availability
            var activeAssignment = await _repository.GetActiveAssignmentAsync(dto.RescueVehicleId);
            if (activeAssignment != null)
            {
                _logger.LogWarning("Vehicle {VehicleId} already assigned to request {ExistingRequestId} (Status: {Status})",
                    dto.RescueVehicleId,
                    activeAssignment.RescueVehicleRequestId);

                throw new InvalidOperationException(
                    $"Vehicle {dto.RescueVehicleId} is currently assigned to request {activeAssignment.RescueVehicleRequestId})");
            }
        }
    }
}
