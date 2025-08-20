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
    public class FirstAidDetailMappingProfile : Profile
    {
        public FirstAidDetailMappingProfile()
        {
            CreateMap<FirstAidDetailCreateInput, FirstAidDetail>()
                .ForMember(d => d.Id, opt => opt.Ignore()) 
                .ForMember(d => d.EmergencySubCategory, opt => opt.Ignore()); 

            CreateMap<FirstAidDetailUpdateInput, FirstAidDetail>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.EmergencySubCategoryId, opt => opt.Ignore()) 
                .ForMember(d => d.EmergencySubCategory, opt => opt.Ignore())
                .ForMember(d => d.DisplayOrder, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<FirstAidDetail, FirstAidDetailCreateInput>();
            CreateMap<FirstAidDetail, FirstAidDetailUpdateInput>();
        }
    }
}
