using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTO;
using Core.Models;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace Application.Mapping
{
    public class RescueVehicleRequestProfile : Profile
    {
        private readonly GeometryFactory _geometryFactory;
        public RescueVehicleRequestProfile()
        {
            _geometryFactory = NtsGeometryServices
                .Instance
                .CreateGeometryFactory(srid: 4326);

            CreateMap<RescueVehicleRequestCreateInput, RescueVehicleRequest>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Status, opt => opt.MapFrom(_ => "Searching"))
                .ForMember(d => d.ProofImageURL, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(d => d.Location, o =>
                                o.MapFrom(src => _geometryFactory.CreatePoint(new Coordinate(src.Longitude, src.Latitude))))
                .ForMember(d => d.EmergencySubCategory, opt => opt.Ignore())
                .ForMember(d => d.Civilian, opt => opt.Ignore())
                .ForMember(d => d.RescueVehicleAssignments, opt => opt.Ignore());

            CreateMap<RescueVehicleRequestUpdateInput, RescueVehicleRequest>()
                .ConstructUsing(src => new RescueVehicleRequest()) 
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
