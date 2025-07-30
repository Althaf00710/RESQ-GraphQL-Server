using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTO;
using Core.Models;

namespace Application.Mapping
{
    public class EmergencyToVehicleProfile : Profile
    {
        public EmergencyToVehicleProfile()
        {
            CreateMap<EmergencyToVehicleInput, EmergencyToVehicle>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EmergencyCategory, opt => opt.Ignore())
                .ForMember(dest => dest.RescueVehicleCategory, opt => opt.Ignore())
                .ReverseMap(); 

        }
    }
}
