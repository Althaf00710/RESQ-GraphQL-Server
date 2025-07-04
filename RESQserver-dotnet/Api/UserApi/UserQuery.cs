﻿using Core.Models;
using Core.Services.Interfaces;
using HotChocolate.Authorization;

namespace RESQserver_dotnet.Api.UserApi
{
    [ExtendObjectType<Query>]
    public class UserQuery
    {
        public async Task<IEnumerable<User>> GetUsers([Service] IUserService userService)
            => await userService.GetAllAsync();

        public async Task<User?> GetUserById(int id, [Service] IUserService userService)
            => await userService.GetByIdAsync(id);

    }
}
