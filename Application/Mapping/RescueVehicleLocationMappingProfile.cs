using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTO;
using Core.models;

namespace Application.Mapping
{
    public class RescueVehicleLocationMappingProfile : Profile
    {
        public RescueVehicleLocationMappingProfile()
        {
            CreateMap<RescueVehicleLocation, RescueVehicleLocationInput>()
                .ReverseMap();
            CreateMap<RescueVehicleLocation, RescueVehicleLocationStatusInput>()
                .ReverseMap();
        }
    }
}
