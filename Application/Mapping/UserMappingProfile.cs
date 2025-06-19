using AutoMapper;
using Core.DTO;
using Core.Models;

namespace Application.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserCreateInput, User>()
                .ForMember(dest => dest.JoinedDate, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.LastActive, opt => opt.MapFrom(_ => (DateTime?)null))
                .ForMember(dest => dest.ProfilePicturePath, opt => opt.Ignore());
        }
    }
    
}
