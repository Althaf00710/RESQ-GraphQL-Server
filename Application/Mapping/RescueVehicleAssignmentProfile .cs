﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTO;
using Core.Models;

namespace Application.Mapping
{
    public class RescueVehicleAssignmentProfile : Profile
    {
        public RescueVehicleAssignmentProfile()
        {
            // Create Mapping
            CreateMap<AssignmentCreateInput, RescueVehicleAssignment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => "Dispatched")) // Default status
                .ForMember(dest => dest.ArrivalTime, opt => opt.Ignore())
                .ForMember(dest => dest.DepartureTime, opt => opt.Ignore())
                .ForMember(dest => dest.RescueVehicleRequest, opt => opt.Ignore())
                .ForMember(dest => dest.RescueVehicle, opt => opt.Ignore());

            // Update Mapping
            CreateMap<AssignmentUpdateInput, RescueVehicleAssignment>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src =>
                    src.Status == "Arrived" ? DateTime.UtcNow : (DateTime?)null))
                .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src =>
                    src.Status == "Completed" ? DateTime.UtcNow : (DateTime?)null));
        }
    }
}
