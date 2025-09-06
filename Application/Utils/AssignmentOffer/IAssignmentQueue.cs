using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.DTO;
using Core.Models;
using Core.Repositories.Interfaces;
using HotChocolate.Subscriptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.Utils.AssignmentOffer
{
    public interface IAssignmentQueue
    {
        void Enqueue(RescueVehicleRequest request, IReadOnlyList<int> vehicleIds);
        Task NotifyResponseAsync(int requestId, int vehicleId, bool accepted);
        void Cancel(int requestId);
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
            if (vehicleIds == null || vehicleIds.Count == 0)
            {
                _logger.LogInformation("---------------------------------No active vehicles nearby for request {RequestId}", request.Id);
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

        public void Cancel(int requestId)
        {
            if (_states.TryRemove(requestId, out var state))
            {
                state.Cancel();
                _logger.LogInformation("Cancelled assignment flow for request {RequestId}", requestId);

                // Inform all vehicles that were offered this request
                _ = Task.Run(() => SendCancellationNotificationsAsync(requestId, state));
            }
            else
            {
                _logger.LogDebug("Cancel called, but no active assignment flow for request {RequestId}", requestId);
            }
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
                var success = await TryAssignBatchRecursiveAsync(request, state, state.VehicleIds);

                if (!success && !state.IsCancelled)
                {
                    _logger.LogInformation("------------------------No vehicle accepted request {RequestId}.", request.Id);
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

        /// <summary>
        /// Try a batch of vehicles sequentially (15s per vehicle). If nobody accepts,
        /// request the nearest 10 and recurse. Respects cancellation at each stage.
        /// </summary>
        private async Task<bool> TryAssignBatchRecursiveAsync(
            RescueVehicleRequest request,
            AssignmentState state,
            IReadOnlyList<int> batch)
        {
            if (state.IsCancelled) return false;

            // Offer each vehicle in this batch
            for (int i = 0; i < batch.Count; i++)
            {
                if (state.IsCancelled) return false;

                var vehicleId = batch[i];
                await PublishOfferAsync(state, vehicleId, request);

                var responded = await state.WaitForResponseAsync(TimeSpan.FromSeconds(15));
                if (state.IsCancelled) return false;

                if (responded && state.Accepted && state.RespondedVehicleId == vehicleId)
                {
                    var accepted = await AcceptAsync(request.Id, vehicleId);
                    if (accepted) return true; // stop on first winner
                }
            }

            if (state.IsCancelled) return false;

            // Nobody accepted -> expand to next 10
            var (lat, lng) = ExtractLatLng(request);
            using var scope = _scopeFactory.CreateScope();
            var locRepo = scope.ServiceProvider.GetRequiredService<IRescueVehicleLocationRepository>();

            var nextIds = await locRepo.GetNearestVehicleIdsAsync(lat, lng, request.EmergencySubCategoryId, maxCount: 10);
            if (nextIds == null || nextIds.Count == 0) return false;

            _logger.LogInformation(
                "Retrying request {RequestId} with {Count} vehicles (repeats allowed).",
                request.Id, nextIds.Count
            );

            return await TryAssignBatchRecursiveAsync(request, state, nextIds);
        }

        // NOTE: signature now accepts 'state' so we can track who was notified
        private async Task PublishOfferAsync(AssignmentState state, int vehicleId, RescueVehicleRequest request)
        {
            var offer = new VehicleAssignmentOffer
            {
                OfferTtlSeconds = 15,
                OfferedAt = DateTimeOffset.Now,
                IsCancelled = false,
                Request = request
            };

            var topic = AssignmentTopics.Vehicle(vehicleId);
            await _sender.SendAsync(topic, offer);

            state.MarkNotified(vehicleId); // track for later cancel broadcasting
            _logger.LogInformation("----------------Offered request {RequestId} to vehicle {VehicleId}", request.Id, vehicleId);
        }

        private async Task PublishCancelAsync(int vehicleId, RescueVehicleRequest request)
        {
            var cancel = new VehicleAssignmentOffer
            {
                OfferTtlSeconds = 0,
                OfferedAt = DateTimeOffset.Now,
                IsCancelled = true,   // 🔴 important
                Request = request
            };

            var topic = AssignmentTopics.Vehicle(vehicleId);
            await _sender.SendAsync(topic, cancel);
            _logger.LogInformation("Sent CANCEL notice for request {RequestId} to vehicle {VehicleId}", request.Id, vehicleId);
        }

        private async Task SendCancellationNotificationsAsync(int requestId, AssignmentState state)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var requestRepo = scope.ServiceProvider.GetRequiredService<IRescueVehicleRequestRepository>();
                var req = await requestRepo.GetByIdAsync(requestId);
                if (req == null) return;

                // Notify every vehicle that previously received an offer for this request
                foreach (var vehicleId in state.GetNotifiedVehicleIds())
                {
                    try
                    {
                        await PublishCancelAsync(vehicleId, req);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed sending CANCEL to vehicle {VehicleId} for request {RequestId}", vehicleId, requestId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while broadcasting cancellation for request {RequestId}", requestId);
            }
        }

        /// <summary>
        /// Returns true if accept succeeded (i.e., request was still "Searching").
        /// </summary>
        private async Task<bool> AcceptAsync(int requestId, int vehicleId)
        {
            using var scope = _scopeFactory.CreateScope();

            var requestRepo = scope.ServiceProvider.GetRequiredService<IRescueVehicleRequestRepository>();
            var assignmentRepo = scope.ServiceProvider.GetRequiredService<IRescueVehicleAssignmentRepository>();

            var req = await requestRepo.GetByIdAsync(requestId);
            if (req is null) return false;

            if (!string.Equals(req.Status, "Searching", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation(
                    "Request {RequestId} already {Status}; ignoring acceptance from vehicle {VehicleId}",
                    requestId, req.Status, vehicleId);
                return false;
            }

            var active = await assignmentRepo.GetActiveAssignmentAsync(vehicleId);
            if (active != null)
            {
                _logger.LogInformation(
                    "Vehicle {VehicleId} already busy with request {ExistingRequestId}; ignoring.",
                    vehicleId, active.RescueVehicleRequestId);
                return false;
            }

            var assignment = new RescueVehicleAssignment
            {
                RescueVehicleRequestId = requestId,
                RescueVehicleId = vehicleId,
                Timestamp = DateTime.Now
            };

            await assignmentRepo.AddAsync(assignment);
            req.Status = "Dispatched";
            await requestRepo.SaveAsync();

            await _sender.SendAsync("VehicleRequestStatusChanged", req);
            await _sender.SendAsync($"VehicleRequestStatusChanged_{req.Id}", req);

            _logger.LogInformation("Request {RequestId} accepted by vehicle {VehicleId}", requestId, vehicleId);
            return true;
        }

        private static (double lat, double lng) ExtractLatLng(RescueVehicleRequest request)
        {
            double lat = 0, lng = 0;
            if (request.Location is not null)
            {
                // NetTopologySuite: X = lon, Y = lat
                lng = request.Location.X;
                lat = request.Location.Y;
            }
            return (lat, lng);
        }

        private sealed class AssignmentState
        {
            private readonly object _gate = new();
            private TaskCompletionSource<(int vehicleId, bool accepted)> _tcs =
                new(TaskCreationOptions.RunContinuationsAsynchronously);

            private readonly CancellationTokenSource _cts = new();
            private readonly ConcurrentDictionary<int, byte> _notified = new(); // track who got offers

            public int RequestId { get; }
            public IReadOnlyList<int> VehicleIds { get; }
            public int Index { get; set; } // retained for compatibility
            public bool Accepted { get; private set; }
            public int RespondedVehicleId { get; private set; }
            public bool IsCancelled => _cts.IsCancellationRequested;

            public AssignmentState(int requestId, IReadOnlyList<int> vehicleIds)
            {
                RequestId = requestId;
                VehicleIds = vehicleIds;
                Index = 0;
            }

            public void Cancel()
            {
                try { _cts.Cancel(); } catch { /* ignore */ }
                lock (_gate)
                {
                    _tcs.TrySetResult((0, false)); // wake any waiters
                }
            }

            public void MarkNotified(int vehicleId) => _notified.TryAdd(vehicleId, 0);
            public IEnumerable<int> GetNotifiedVehicleIds() => _notified.Keys;

            public bool TrySetResponse(int vehicleId, bool accepted)
            {
                lock (_gate)
                {
                    if (_tcs.Task.IsCompleted || IsCancelled) return false;
                    Accepted = accepted;
                    RespondedVehicleId = vehicleId;
                    _tcs.TrySetResult((vehicleId, accepted));
                    return true;
                }
            }

            public async Task<bool> WaitForResponseAsync(TimeSpan timeout)
            {
                try
                {
                    var completed = await Task.WhenAny(_tcs.Task, Task.Delay(timeout, _cts.Token));
                    lock (_gate)
                    {
                        var hadResponse = !IsCancelled && completed == _tcs.Task;

                        // Prepare for next cycle
                        _tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
                        if (!hadResponse)
                        {
                            Accepted = false;
                            RespondedVehicleId = 0;
                        }

                        return hadResponse;
                    }
                }
                catch (TaskCanceledException)
                {
                    return false;
                }
            }
        }
    }
}
