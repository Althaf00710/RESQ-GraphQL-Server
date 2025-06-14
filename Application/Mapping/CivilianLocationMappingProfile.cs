using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Application.Mapping
{
    public class CivilianLocationMappingProfile : Profile
    {
        public CivilianLocationMappingProfile()
        {
            CreateMap<Core.Models.CivilianLocation, Core.DTO.CivilianLocationInput>()
                .ReverseMap();
        
            CreateMap<Core.Models.CivilianLocation, Core.DTO.CivilianLocationStatusInput>()
                .ReverseMap();
        }
    }
}
