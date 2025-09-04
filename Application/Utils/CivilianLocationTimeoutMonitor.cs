using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Utils
{
    public class CivilianLocationTimeoutMonitor : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CivilianLocationTimeoutMonitor> _logger;
        private Timer _timer;

        private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(5);
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);

        public CivilianLocationTimeoutMonitor(
            IServiceScopeFactory scopeFactory,
            ILogger<CivilianLocationTimeoutMonitor> logger)
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

            foreach (var kv in Application.Services.CivilianLocationService._lastSeen)
            {
                var civilianId = kv.Key;
                var lastSeen = kv.Value;

                if (now - lastSeen <= _timeout) continue;

                _logger.LogWarning("No update for civilian {id} in {sec}s — marking inactive",
                                   civilianId, _timeout.TotalSeconds);

                Task.Run(async () =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var svc = scope.ServiceProvider.GetRequiredService<ICivilianLocationService>();

                    await svc.MarkInactiveAsync(civilianId);

                    // prevent re-trigger
                    Application.Services.CivilianLocationService._lastSeen.TryRemove(civilianId, out _);
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
}
