using System.Collections.Concurrent;
using Application.Utils.AssignmentOffer;
using Core.DTO;
using Core.Models;
using Core.Repositories.Interfaces;
using HotChocolate.Subscriptions;
using Microsoft.Extensions.DependencyInjection; // <-- for IServiceScopeFactory
using Microsoft.Extensions.Logging;

namespace Application.Utils.AssignmentOffer
{
    public interface IAssignmentQueue
    {
        void Enqueue(RescueVehicleRequest request, IReadOnlyList<int> vehicleIds);
        Task NotifyResponseAsync(int requestId, int vehicleId, bool accepted);
    }

    internal sealed class AssignmentQueue : IAssignmentQueue
    {
        private readonly ITopicEventSender _sender;
        private readonly ILogger<AssignmentQueue> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly ConcurrentDictionary<int, AssignmentState> _states = new();

        public AssignmentQueue(
            ITopicEventSender sender,
            ILogger<AssignmentQueue> logger,
            IServiceScopeFactory scopeFactory)
        {
            _sender = sender;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public void Enqueue(RescueVehicleRequest request, IReadOnlyList<int> vehicleIds)
        {
            if (vehicleIds.Count == 0)
            {
                _logger.LogInformation("No active vehicles nearby for request {RequestId}", request.Id);
                return;
            }

            var state = new AssignmentState(request.Id, vehicleIds);
            if (!_states.TryAdd(request.Id, state))
            {
                _logger.LogWarning("Request {RequestId} is already enqueued.", request.Id);
                return;
            }

            _ = ProcessAsync(request, state); // fire-and-forget
        }

        public Task NotifyResponseAsync(int requestId, int vehicleId, bool accepted)
        {
            if (_states.TryGetValue(requestId, out var state))
            {
                state.TrySetResponse(vehicleId, accepted);
            }
            return Task.CompletedTask;
        }

        private async Task ProcessAsync(RescueVehicleRequest request, AssignmentState state)
        {
            try
            {
                for (; state.Index < state.VehicleIds.Count; state.Index++)
                {
                    var vehicleId = state.VehicleIds[state.Index];

                    await PublishOfferAsync(vehicleId, request);

                    var responded = await state.WaitForResponseAsync(TimeSpan.FromSeconds(15));

                    if (responded && state.Accepted && state.RespondedVehicleId == vehicleId)
                    {
                        var accepted = await AcceptAsync(request.Id, vehicleId);
                        if (accepted) break; // stop on first winner
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Assignment flow failed for request {RequestId}", request.Id);
            }
            finally
            {
                _states.TryRemove(request.Id, out _);
            }
        }

        private async Task PublishOfferAsync(int vehicleId, RescueVehicleRequest request)
        {
            double lat = 0, lng = 0;
            if (request.Location is not null)
            {
                lng = request.Location.X; // X = lon
                lat = request.Location.Y; // Y = lat
            }

            var offer = new VehicleAssignmentOffer
            {
                RequestId = request.Id,
                VehicleId = vehicleId,
                Location = request.Address,  // string address
                Latitude = lat,
                Longitude = lng,
                OfferTtlSeconds = 15,
                OfferedAt = DateTimeOffset.Now
            };

            var topic = AssignmentTopics.Vehicle(vehicleId);
            await _sender.SendAsync(topic, offer);
            _logger.LogInformation("Offered request {RequestId} to vehicle {VehicleId}", request.Id, vehicleId);
        }

        // Returns true if accept succeeded (i.e., request was still Searching)
        private async Task<bool> AcceptAsync(int requestId, int vehicleId)
        {
            using var scope = _scopeFactory.CreateScope();

            var requestRepo = scope.ServiceProvider.GetRequiredService<IRescueVehicleRequestRepository>();
            var assignmentRepo = scope.ServiceProvider.GetRequiredService<IRescueVehicleAssignmentRepository>();

            // Load the request fresh inside this scope
            var req = await requestRepo.GetByIdAsync(requestId);
            if (req is null) return false;

            // Concurrency guard: only accept if still "Searching"
            if (!string.Equals(req.Status, "Searching", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation(
                    "Request {RequestId} already {Status}; ignoring acceptance from vehicle {VehicleId}",
                    requestId, req.Status, vehicleId);
                return false;
            }

            // Optional: ensure vehicle has no active assignment
            var active = await assignmentRepo.GetActiveAssignmentAsync(vehicleId);
            if (active != null)
            {
                _logger.LogInformation(
                    "Vehicle {VehicleId} already busy with request {ExistingRequestId}; ignoring.",
                    vehicleId, active.RescueVehicleRequestId);
                return false;
            }

            // Create assignment + move request to Dispatched
            var assignment = new RescueVehicleAssignment
            {
                RescueVehicleRequestId = requestId,
                RescueVehicleId = vehicleId,
                Timestamp = DateTime.Now
            };

            await assignmentRepo.AddAsync(assignment);
            req.Status = "Dispatched";

            // Persist both (shared DbContext means one save is fine, but calling once here is safe)
            await requestRepo.SaveAsync();

            // Broadcast status change for UIs
            await _sender.SendAsync("VehicleRequestStatusChanged", req);

            _logger.LogInformation("Request {RequestId} accepted by vehicle {VehicleId}", requestId, vehicleId);
            return true;
        }

        private sealed class AssignmentState
        {
            private readonly object _gate = new();
            private TaskCompletionSource<(int vehicleId, bool accepted)> _tcs =
                new(TaskCreationOptions.RunContinuationsAsynchronously);

            public int RequestId { get; }
            public IReadOnlyList<int> VehicleIds { get; }
            public int Index { get; set; }
            public bool Accepted { get; private set; }
            public int RespondedVehicleId { get; private set; }

            public AssignmentState(int requestId, IReadOnlyList<int> vehicleIds)
            {
                RequestId = requestId;
                VehicleIds = vehicleIds;
                Index = 0;
            }

            public bool TrySetResponse(int vehicleId, bool accepted)
            {
                lock (_gate)
                {
                    if (_tcs.Task.IsCompleted) return false;
                    Accepted = accepted;
                    RespondedVehicleId = vehicleId;
                    _tcs.TrySetResult((vehicleId, accepted));
                    return true;
                }
            }

            public async Task<bool> WaitForResponseAsync(TimeSpan timeout)
            {
                var completed = await Task.WhenAny(_tcs.Task, Task.Delay(timeout));
                lock (_gate)
                {
                    var hadResponse = completed == _tcs.Task;
                    _tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
                    if (!hadResponse)
                    {
                        Accepted = false;
                        RespondedVehicleId = 0;
                    }
                    return hadResponse;
                }
            }
        }
    }
}
