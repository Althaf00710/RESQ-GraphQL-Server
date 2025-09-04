// Application/Services/CivilianLocationService.cs
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
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace Application.Services
{
    public class CivilianLocationService : Service<CivilianLocation>, ICivilianLocationService
    {
        // track last-seen time for timeout detection (per civilian)
        internal static readonly ConcurrentDictionary<int, DateTime> _lastSeen = new();

        private readonly ICivilianLocationRepository _repository;
        private readonly ILogger<CivilianLocationService> _logger;
        private readonly IMapper _mapper;
        private readonly ITopicEventSender _sender;

        public const string Topic = "CivilianLocationShare";

        public CivilianLocationService(
            ICivilianLocationRepository repository,
            ILogger<CivilianLocationService> logger,
            IMapper mapper,
            ITopicEventSender sender
        ) : base(repository)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _sender = sender;
        }

        public async Task<CivilianLocation> Handle(CivilianLocationInput dto)
        {
            if (dto is null)
            {
                _logger.LogWarning("Received null CivilianLocationInput DTO");
                throw new ArgumentNullException(nameof(dto));
            }

            var now = DateTime.Now;
            _lastSeen.AddOrUpdate(dto.CivilianId, now, (_, __) => now);

            // Single-record per civilian
            var existing = await _repository.GetByCivilianId(dto.CivilianId);

            if (existing is null)
            {
                // insert once
                var entity = _mapper.Map<CivilianLocation>(dto);
                entity.Active = true;
                entity.LastActive = now;

                await _repository.AddAsync(entity);
                await _repository.SaveAsync();

                await _sender.SendAsync(Topic, entity);
                _logger.LogInformation("Created location for civilian {id}", dto.CivilianId);
                return entity;
            }
            else
            {
                // update in place (no new rows)
                existing.Active = true;
                existing.LastActive = now;
                existing.Address = dto.Address;

                // NTS uses X=Longitude, Y=Latitude; SRID 4326
                var gf = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                existing.Location = gf.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude));

                await _repository.Update(existing);   // ok tracked or detached
                await _repository.SaveAsync();

                await _sender.SendAsync(Topic, existing);
                _logger.LogInformation("Updated location for civilian {id}", dto.CivilianId);
                return existing;
            }
        }

        public async Task MarkInactiveAsync(int civilianId)
        {
            var existing = await _repository.GetByCivilianId(civilianId);
            if (existing is null || !existing.Active) return;

            existing.Active = false;
            existing.LastActive = DateTime.Now;

            await _repository.Update(existing);
            await _repository.SaveAsync();

            await _sender.SendAsync(Topic, existing);
            _logger.LogInformation("Marked civilian {id} inactive after timeout", civilianId);
        }

        public async Task<CivilianLocation> GetByCivilianId(int civilianId)
        {
            var location = await _repository.GetByCivilianId(civilianId);
            if (location is null)
            {
                _logger.LogWarning("No location found for civilian {id}", civilianId);
                throw new Exception($"Location for civilian {civilianId} not found");
            }
            return location;
        }

        public static class Topics
        {
            public static string CivilianNearby(int civilianId) => $"CivilianNearby:{civilianId}";
        }
    }
}
