using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class LocationTimeoutMonitor : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<LocationTimeoutMonitor> _logger;
    private Timer _timer;
    private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(5);
    private readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);

    public LocationTimeoutMonitor(
        IServiceScopeFactory scopeFactory,
        ILogger<LocationTimeoutMonitor> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(CheckForTimeouts, null, _checkInterval, _checkInterval);
        return Task.CompletedTask;
    }

    private void CheckForTimeouts(object? state)
    {
        var now = DateTime.Now;

        // _lastSeen is your static ConcurrentDictionary<int,DateTime>
        foreach (var kv in RescueVehicleLocationService._lastSeen)
        {
            var vehicleId = kv.Key;
            var lastSeen = kv.Value;

            if (now - lastSeen <= _timeout)
                continue;

            _logger.LogWarning("No update for vehicle {id} in {seconds}s — marking inactive",
                                vehicleId, _timeout.TotalSeconds);

            // fire‐and‐forget the work in a scope
            Task.Run(async () =>
            {
                using var scope = _scopeFactory.CreateScope();
                var svc = scope.ServiceProvider
                               .GetRequiredService<IRescueVehicleLocationService>();

                await svc.MarkInactiveAsync(vehicleId);

                // remove so we don’t re‐trigger
                RescueVehicleLocationService._lastSeen.TryRemove(vehicleId, out _);
            });
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose() => _timer?.Dispose();
}

