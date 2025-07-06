using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Generic;
using Application.Utils;
using AutoMapper;
using Core.DTO;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    internal class SnakeService : Service<Snake>, ISnakeService
    {
        private readonly ISnakeRepository _repository;
        private readonly ILogger<Snake> _logger;
        private readonly IMapper _mapper;

        public SnakeService(ISnakeRepository repository, ILogger<Snake> logger, IMapper mapper) : base(repository)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;   
        }

        public async Task<Snake> Add(SnakeCreateInput dto, IFile? picture)
        {
            if (await CheckExist(dto.ScientificName))
            {
                _logger.LogWarning("Snake with scientific name {ScientificName} already exists", dto.ScientificName);
                throw new Exception("Snake with this scientific name already exists");
            }

            var snake = _mapper.Map<Snake>(dto);

            if (picture is not null)
            {
                snake.ImageUrl = await FileHandler.StoreImage("snakes", picture);
                _logger.LogInformation("Snake image stored at: {ImageUrl}", snake.ImageUrl);
            }

            try
            {
                await _repository.AddAsync(snake);
                await _repository.SaveAsync();

                _logger.LogInformation("Snake created successfully with ID: {Id}", snake.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating snake with scientific name: {ScientificName}", dto.ScientificName);

                // Clean up stored image if creation failed
                if (!string.IsNullOrEmpty(snake.ImageUrl))
                {
                    FileHandler.DeleteImage(snake.ImageUrl);
                }

                throw;
            }

            return snake;
        }

        public async Task<Snake> Update(int id, SnakeUpdateInput dto, IFile? picture)
        {
            var snake = await _repository.GetByIdAsync(id);
            if (snake == null)
            {
                _logger.LogWarning("Snake not found: ID {Id}", id);
                throw new Exception($"Snake with ID {id} not found");
            }

            if (!string.IsNullOrEmpty(dto.ScientificName) && dto.ScientificName != snake.ScientificName)
            {
                if (await CheckExist(dto.ScientificName))
                {
                    _logger.LogWarning("Duplicate scientific name: {ScientificName}", dto.ScientificName);
                    throw new Exception($"Snake with scientific name '{dto.ScientificName}' already exists");
                }
            }

            _mapper.Map(dto, snake);

            if (picture != null)
            {
                try
                {
                    var newImageUrl = await FileHandler.StoreImage("snakes", picture);

                    // Only delete old image after successful upload
                    if (!string.IsNullOrEmpty(snake.ImageUrl))
                    {
                        FileHandler.DeleteImage(snake.ImageUrl);
                    }

                    snake.ImageUrl = newImageUrl;
                    _logger.LogDebug("Updated snake image to: {ImageUrl}", newImageUrl);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to update image for snake ID {Id}", id);
                    throw new ApplicationException("Failed to update snake image", ex);
                }
            }

            try
            {
                await _repository.SaveAsync();
                _logger.LogInformation("Successfully updated snake ID {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save updates for snake ID {Id}", id);
                throw new ApplicationException("Failed to update snake record", ex);
            }

            return snake;
        }

        private async Task<bool> CheckExist(string scientificName)
        {
            if (string.IsNullOrWhiteSpace(scientificName))
            {
                _logger.LogWarning("CheckExist called with empty scientific name");
                throw new ArgumentException("Scientific name cannot be empty", nameof(scientificName));
            }

            try
            {
                return await _repository.CheckExist(scientificName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking existence of snake with scientific name: {ScientificName}", scientificName);
                throw;
            }
        }
    }
}
