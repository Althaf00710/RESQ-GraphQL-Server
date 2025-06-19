using AutoMapper;
using Core.Models;
using Core.DTO;

namespace Application.Mapping
{
    public class EmergencyCategoryMappingProfile : Profile
    {
        public EmergencyCategoryMappingProfile()
        {
            CreateMap<EmergencyCategoryCreateInput, EmergencyCategory>();

            CreateMap<EmergencyCategoryUpdateInput, EmergencyCategory>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<EmergencyCategory, EmergencyCategoryCreateInput>();
            CreateMap<EmergencyCategory, EmergencyCategoryUpdateInput>();
        }
    }
}
