﻿using Core.DTO;
using Core.Models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface ICivilianLocationService : IService<CivilianLocation>
    {
        Task<CivilianLocation> Handle(CivilianLocationInput dto);
        Task<CivilianLocation> GetByCivilianId(int civilianId);
    }
}
