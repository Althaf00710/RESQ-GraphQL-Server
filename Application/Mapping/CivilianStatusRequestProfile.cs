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
    public class CivilianStatusRequestProfile : Profile
    {
        public CivilianStatusRequestProfile() { 
            CreateMap<CivilianStatusRequestCreateInput, CivilianStatusRequest>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.status, opt => opt.MapFrom(_ => "Pending"));
        }
    }
}
