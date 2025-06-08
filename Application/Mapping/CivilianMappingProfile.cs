using AutoMapper;
using Core.DTO;
using Core.Models;

namespace Application.Mapping
{
    public class CivilianMappingProfile : Profile
    {
        public CivilianMappingProfile()
        {
            CreateMap<CivilianCreateInput, Civilian>()
                .ForMember(dest => dest.JoinedDate, opt => opt.MapFrom(_ => DateTime.Now));
        }
    }
}
