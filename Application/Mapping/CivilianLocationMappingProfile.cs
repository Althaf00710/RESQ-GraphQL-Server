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
    public class CivilianLocationMappingProfile : Profile
    {
        public CivilianLocationMappingProfile()
        {
            CreateMap<CivilianLocation, CivilianLocationInput>()
                .ReverseMap();
        
            CreateMap<CivilianLocation, CivilianLocationStatusInput>()
                .ReverseMap();
        }
    }
}
