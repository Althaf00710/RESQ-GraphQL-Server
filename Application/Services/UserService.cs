using Application.Services.Generic;
using Application.Utils;
using AutoMapper;
using Core.DTO;
using Core.models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class UserService : Service<User>, IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        public UserService(IUserRepository repository, ILogger<UserService> logger, IMapper mapper) : base(repository)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<User> Add(UserCreateInput dto, IFile? profilePicture)
        {
            _logger.LogInformation("Attempting to add new user with email: {Email}", dto.Email);

            if (await _repository.GetByEmailAsync(dto.Email) is not null)
            {
                _logger.LogWarning("User with email {Email} already exists", dto.Email);
                throw new Exception("User Email already exists");
            }

            if (await _repository.GetByUsernameAsync(dto.Username) is not null)
            {
                _logger.LogWarning("User with username {Username} already exists", dto.Username);
                throw new Exception("Username already exists");
            }

            var user = _mapper.Map<User>(dto);

            if (profilePicture is not null) 
                user.ProfilePicturePath = await FileHandler.StoreImage("users", profilePicture);

            try
            {
                user.Password = PasswordHash.HashPassword(user.Password);
                await _repository.AddAsync(user);
                await _repository.SaveAsync();

                _logger.LogInformation("User created successfully", user.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user", dto.Email);
                throw;
            }

            return user;
        }

        public async Task<User> Update(int id, UserUpdateInput dto, IFile? profilePicture)
        {
            _logger.LogInformation("Attempting to update user: {@Dto}", dto);

            var user = await _repository.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {Id} not found", id);
                throw new Exception("User not found");
            }

            if (dto.Email != null && dto.Email != user.Email)
            {
                if (await _repository.GetByEmailAsync(dto.Email) is not null)
                {
                    _logger.LogWarning("User with email {Email} already exists", dto.Email);
                    throw new Exception("User email already exists");
                }
                user.Email = dto.Email;
            }

            if (dto.Username != null && dto.Username != user.Username)
            {
                if (await _repository.GetByUsernameAsync(dto.Username) is not null)
                {
                    _logger.LogWarning("User with username {Username} already exists", dto.Username);
                    throw new Exception("Username already exists");
                }
                user.Username = dto.Username;
            }

            if (dto.Name != null) user.Name = dto.Name;
            if (dto.Password != null) user.Password = PasswordHash.HashPassword(dto.Password);

            if (profilePicture is not null)
            {
                try
                {
                    var newPath = await FileHandler.StoreImage("users", profilePicture);

                    // Delete old image only after new image is stored successfully
                    if (!string.IsNullOrEmpty(user.ProfilePicturePath))
                    {
                        var deleted = FileHandler.DeleteImage(user.ProfilePicturePath);
                        _logger.LogInformation("Old profile picture deleted: {Result}", deleted);
                    }

                    user.ProfilePicturePath = newPath;
                    _logger.LogInformation("New profile picture stored at: {Path}", newPath);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to store new profile picture. Skipping image update.");
                }
            }

            try
            {
                await _repository.SaveAsync();
                _logger.LogInformation("User updated successfully: {@User}", user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user: {@Dto}", dto);
                throw;
            }

            return user;
        }


    }
}
