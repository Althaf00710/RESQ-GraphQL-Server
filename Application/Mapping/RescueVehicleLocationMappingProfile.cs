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
    public class RescueVehicleLocationMappingProfile : Profile
    {
        private readonly GeometryFactory _geometryFactory;
        public RescueVehicleLocationMappingProfile()
        {
            // WGS84 (EPSG:4326) factory
            _geometryFactory = NtsGeometryServices
                .Instance
                .CreateGeometryFactory(srid: 4326);

            CreateMap<RescueVehicleLocationInput, RescueVehicleLocation>()
                .ForMember(dst => dst.RescueVehicleId, opt => opt.MapFrom(src => src.RescueVehicleId))
                .ForMember(dst => dst.Address, opt => opt.MapFrom(src => src.Address))

                // point
                .ForMember(
                    dst => dst.Location,
                    opt => opt.MapFrom(src =>
                        _geometryFactory.CreatePoint(
                            new Coordinate(src.Longitude, src.Latitude)
                        )
                    )
                )
                .ForMember(dst => dst.LastActive,
                           opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dst => dst.Active, _ => _.Ignore())
                .ForMember(dst => dst.Id, _ => _.Ignore());
        }
    }
}
