// Application/Services/RescueVehicleLocationService.cs
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Application.Services.Generic;
using AutoMapper;
using Core.DTO;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using HotChocolate.Subscriptions;
using Microsoft.Extensions.Logging;

public class RescueVehicleLocationService
    : Service<RescueVehicleLocation>, IRescueVehicleLocationService
{
    // track whether we've persisted the “initial” record for each vehicle
    private static readonly ConcurrentDictionary<int, bool> _hasInitial
        = new();

    // track last‐seen time for timeout detection
    internal static readonly ConcurrentDictionary<int, DateTime> _lastSeen
        = new();

    private readonly IRescueVehicleLocationRepository _repository;
    private readonly ILogger<RescueVehicleLocationService> _logger;
    private readonly IMapper _mapper;
    private readonly ITopicEventSender _sender;

    public RescueVehicleLocationService(
        IRescueVehicleLocationRepository repository,
        ILogger<RescueVehicleLocationService> logger,
        IMapper mapper,
        ITopicEventSender sender
    ) : base(repository)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _sender = sender;
    }

    public async Task<RescueVehicleLocation> Handle(RescueVehicleLocationInput dto)
    {
        if (dto is null) throw new ArgumentNullException(nameof(dto));

        var now = DateTime.UtcNow;
        _lastSeen.AddOrUpdate(dto.RescueVehicleId, now, (_, __) => now);

        // 1) Try to load the existing single row for this vehicle
        var existing = await _repository.GetByRescueVehicleId(dto.RescueVehicleId);

        if (existing is null)
        {
            // 2) Not found -> insert once
            var entity = _mapper.Map<RescueVehicleLocation>(dto);
            entity.Active = true;
            entity.LastActive = now;

            await _repository.AddAsync(entity);
            await _repository.SaveAsync();

            await _sender.SendAsync("VehicleLocationShare", entity);
            return entity;
        }
        else
        {
            // 3) Found -> update in place (do NOT create a new row)
            // Update only the fields that should change
            existing.Active = true;
            existing.LastActive = now;
            existing.Address = dto.Address;

            // If you store geometry with NTS, assign the mapped point/geometry here:
            // existing.Location = _mapper.Map<NetTopologySuite.Geometries.Geometry>(dto.Location);
            // (or however your mapper is set up)

            _repository.Update(existing); // no-op if tracked; ok if detached with EF Core
            await _repository.SaveAsync();

            await _sender.SendAsync("VehicleLocationShare", existing);
            return existing;
        }
    }

    public async Task MarkInactiveAsync(int rescueVehicleId)
    {
        var existing = await _repository.GetByRescueVehicleId(rescueVehicleId);
        if (existing == null || !existing.Active) return;

        existing.Active = false;
        // keep LastActive as the last ping time; don't overwrite unless you want the timeout moment
        _repository.Update(existing);
        await _repository.SaveAsync();

        _logger.LogInformation("Marked vehicle {id} inactive after timeout", rescueVehicleId);
        await _sender.SendAsync("VehicleLocationShare", existing);
    }
}
