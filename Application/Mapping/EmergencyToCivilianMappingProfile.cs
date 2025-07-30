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
    public class EmergencyToCivilianProfile : Profile
    {
        public EmergencyToCivilianProfile()
        {
            // DTO -> Entity
            CreateMap<EmergencyToCivilianInput, EmergencyToCivilian>()
                // don't try to map the PK (database will generate it)
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                // navigation props are populated separately
                .ForMember(dest => dest.EmergencyCategory, opt => opt.Ignore())
                .ForMember(dest => dest.CivilianStatus, opt => opt.Ignore())
                .ReverseMap(); // also sets up Entity -> DTO mapping
        }
    }
}
