using Core.Models;
using HotChocolate.Types;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using RESQserver_dotnet.Api.CivilianApi;
using System.Threading.Tasks;

namespace RESQserver_dotnet.Api.CivilianLocationApi
{
    public class CivilianLocationType : ObjectType<CivilianLocation>
    {
        protected override void Configure(IObjectTypeDescriptor<CivilianLocation> descriptor)
        {
            descriptor.Description("Represents a civilian's geo‐location and address in the emergency response system.");

            descriptor.Field(cl => cl.Id)
                .Type<NonNullType<IdType>>()
                .Description("Primary key of the location record.");

            descriptor.Field(cl => cl.CivilianId)
                .Type<NonNullType<IntType>>()
                .Description("FK to the civilian.");

            descriptor.Field(cl => cl.Address)
                .Type<NonNullType<StringType>>()
                .Description("Human‐readable address or description.");

            descriptor.Field(cl => cl.Active)
                .Type<NonNullType<BooleanType>>()
                .Description("Whether this is the civilian's current active location.");

            // Expose the Point as latitude & longitude 
            descriptor.Field("latitude")
                .Type<NonNullType<FloatType>>()
                .Description("Latitude (Y) of the Point.")
                .Resolve(ctx => ((Point)ctx.Parent<CivilianLocation>().Location).Y);

            descriptor.Field("longitude")
                .Type<NonNullType<FloatType>>()
                .Description("Longitude (X) of the Point.")
                .Resolve(ctx => ((Point)ctx.Parent<CivilianLocation>().Location).X);

            descriptor.Field(cl => cl.Civilian)
                .Type<NonNullType<CivilianType>>()
                .Description("The associated civilian.")
                .ResolveWith<Resolvers>(r => r.GetCivilianAsync(default!, default!));
        }

        private class Resolvers
        {
            public async Task<Civilian> GetCivilianAsync(
                [Parent] CivilianLocation loc,
                [Service] AppDbContext db)
            {
                return await db.Civilians
                    .AsNoTracking()
                    .FirstAsync(c => c.Id == loc.CivilianId);
            }
        }
    }
}
