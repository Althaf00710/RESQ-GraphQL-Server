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
    public class SnakeMappingProfile : Profile
    {
        public SnakeMappingProfile()
        {
            // SnakeCreateInput -> Snake
            CreateMap<SnakeCreateInput, Snake>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore Id as it's auto-generated
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore()); // ImageUrl is handled separately

            // SnakeUpdateInput -> Snake
            CreateMap<SnakeUpdateInput, Snake>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null)); // Only map properties that are not null
        }
    }
}
