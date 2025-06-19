using AutoMapper;
using Core.DTO;
using Core.Models;

namespace Application.Mapping
{
    public class EmergencySubCategoryMappingProfile : Profile
    {
        public EmergencySubCategoryMappingProfile()
        {
            CreateMap<EmergencySubCategoryCreateInput, EmergencySubCategory>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.EmergencyCategory, opt => opt.Ignore()) 
                .ForMember(dest => dest.FirstAidDetails, opt => opt.Ignore()); 
                

            CreateMap<EmergencySubCategoryUpdateInput, EmergencySubCategory>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EmergencyCategoryId, opt => opt.Ignore()) // Protect from changes
                .ForMember(dest => dest.EmergencyCategory, opt => opt.Ignore())
                .ForMember(dest => dest.FirstAidDetails, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null)); // Only update if value is provided

            CreateMap<EmergencySubCategory, EmergencySubCategoryCreateInput>();
            CreateMap<EmergencySubCategory, EmergencySubCategoryUpdateInput>();
        }
    }
}