// Application/Mapping/CivilianLocationMappingProfile.cs
using System;
using AutoMapper;
using Core.DTO;
using Core.Models;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace Application.Mapping
{
    public class CivilianLocationMappingProfile : Profile
    {
        private readonly GeometryFactory _geometryFactory;

        public CivilianLocationMappingProfile()
        {
            _geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            // Input -> Entity
            CreateMap<CivilianLocationInput, CivilianLocation>()
                .ForMember(dst => dst.CivilianId, opt => opt.MapFrom(src => src.CivilianId))
                .ForMember(dst => dst.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dst => dst.Location, opt => opt.MapFrom(src =>
                    _geometryFactory.CreatePoint(new Coordinate(src.Longitude, src.Latitude))
                ))
                .ForMember(dst => dst.LastActive, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dst => dst.Active, opt => opt.Ignore())
                .ForMember(dst => dst.Id, opt => opt.Ignore());

            // Optional reverse for reads
            CreateMap<CivilianLocation, CivilianLocationInput>()
                .ForMember(dst => dst.Latitude, opt => opt.MapFrom(src => src.Location.Y))
                .ForMember(dst => dst.Longitude, opt => opt.MapFrom(src => src.Location.X));
        }
    }
}
