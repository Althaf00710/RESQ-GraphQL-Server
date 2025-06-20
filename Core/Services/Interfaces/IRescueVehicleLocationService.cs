﻿using Core.DTO;
using Core.Models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface IRescueVehicleLocationService : IService<RescueVehicleLocation>
    {
        Task<RescueVehicleLocation> Handle(RescueVehicleLocationInput dto);
        Task<RescueVehicleLocation> GetByRescueVehicleId(int rescueVehicleId);
    }
}
