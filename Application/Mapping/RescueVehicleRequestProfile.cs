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
    public class RescueVehicleRequestProfile : Profile
    {
        public RescueVehicleRequestProfile()
        {
            CreateMap<RescueVehicleRequestCreateInput, RescueVehicleRequest>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Searching")) // Default status
                .ForMember(dest => dest.ProofImageURL, opt => opt.MapFrom(src => src.ProofImage))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.EmergencyCategory, opt => opt.Ignore())
                .ForMember(dest => dest.Civilian, opt => opt.Ignore())
                .ForMember(dest => dest.RescueVehicleAssignments, opt => opt.Ignore());

            CreateMap<RescueVehicleRequestUpdateInput, RescueVehicleRequest>()
                .ConstructUsing(src => new RescueVehicleRequest()) 
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
