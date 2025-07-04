﻿using Core.Models;

namespace RESQserver_dotnet.Api.CivilianStatusApi
{
    public class CivilianStatusPayload
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public CivilianStatus? CivilianStatus { get; set; }

        public CivilianStatusPayload(bool success, string? message = null, CivilianStatus? civilianStatus = null)
        {
            Success = success;
            Message = message;
            CivilianStatus = civilianStatus;
        }
    }
}
