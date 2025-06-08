using System.Reflection.Metadata.Ecma335;
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
        private readonly JwtTokenGenerator _jwt;

        public UserService(IUserRepository repository, ILogger<UserService> logger, IMapper mapper, JwtTokenGenerator jwt) : base(repository)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _jwt = jwt;
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
                user.LastActive = "Offline";
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

        public async Task<String> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Username and password must not be empty.");
            }

            var user = await _repository.GetByUsernameAsync(username);

            if (user is null)
            {
                _logger.LogWarning("Login failed: User with username '{Username}' not found", username);
                throw new Exception("Invalid username or password.");
            }

            var isPasswordValid = PasswordHash.VerifyPassword(password, user.Password);

            if (!isPasswordValid)
            {
                _logger.LogWarning("Login failed: Invalid password for username '{Username}'", username);
                throw new Exception("Invalid username or password.");
            }
            user.LastActive = "Online";
            await _repository.SaveAsync();
            _logger.LogInformation("User '{Username}' tokken generated successfully", username);

            return _jwt.GenerateToken(user.Id.ToString(), user.Username, "User");
        }



    }
}
