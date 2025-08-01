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
    public class RescueVehicleMappingProfile : Profile
    {
        public RescueVehicleMappingProfile()
        {
            CreateMap<RescueVehicleCreateInput, RescueVehicle>()
                .ForMember(dest => dest.Code, opt => opt.Ignore()) // Will be set in service
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Inactive"))
                .ForMember(dest => dest.RescueVehicleLocations, opt => opt.Ignore())
                .ForMember(dest => dest.RescueVehicleAssignment, opt => opt.Ignore());

            CreateMap<RescueVehicleUpdateInput, RescueVehicle>()
                .ForMember(dest => dest.Code, opt => opt.Ignore())
                .ForMember(dest => dest.RescueVehicleCategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.RescueVehicleLocations, opt => opt.Ignore())
                .ForMember(dest => dest.RescueVehicleAssignment, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null &&
                    !string.IsNullOrWhiteSpace(srcMember.ToString())));

        }
    }
}
