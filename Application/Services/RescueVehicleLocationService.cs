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
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var now = DateTime.UtcNow;
        _lastSeen.AddOrUpdate(dto.RescueVehicleId, now, (_, __) => now);

        // FIRST‐ever update: persist it as the “initial” record
        if (!_hasInitial.ContainsKey(dto.RescueVehicleId))
        {
            var entity = _mapper.Map<RescueVehicleLocation>(dto);
            entity.Active = true;
            await _repository.AddAsync(entity);
            await _repository.SaveAsync();
            _hasInitial[dto.RescueVehicleId] = true;

            // broadcast
            await _sender.SendAsync(
              "VehicleLocationShare",
              entity
            );
            return entity;
        }

        // Normal streaming update: just broadcast, DO NOT Save
        var temp = _mapper.Map<RescueVehicleLocation>(dto);
        temp.Active = true; // still active
        await _sender.SendAsync(
          "VehicleLocationShare",
          temp
        );
        return temp;
    }

    /// <summary>
    /// Called by a background monitor when a timeout is detected.
    /// </summary>
    public async Task MarkInactiveAsync(int rescueVehicleId)
    {
        var existing = await _repository.GetByRescueVehicleId(rescueVehicleId);
        if (existing == null || !existing.Active) return;

        existing.Active = false;
        await _repository.SaveAsync();

        _logger.LogInformation(
            "Marked vehicle {id} inactive after timeout", rescueVehicleId
        );

        // broadcast the “inactive” state
        await _sender.SendAsync(
          "VehicleLocationShare",
          existing
        );
    }
}
