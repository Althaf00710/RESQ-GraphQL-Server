using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using RESQserver_dotnet.Api.CivilianApi;

namespace RESQserver_dotnet.Api.CivilianLocationApi
{
    public class CivilianLocationType : ObjectType<CivilianLocation>
    {
        protected override void Configure(IObjectTypeDescriptor<CivilianLocation> descriptor)
        {
            descriptor.Description("Represents a civilian's location in the emergency response system");

            // Configure scalar fields
            descriptor.Field(cl => cl.Id)
                .Description("The unique identifier of the location record")
                .Type<NonNullType<IdType>>();

            descriptor.Field(cl => cl.CivilianId)
                .Description("The ID of the civilian this location belongs to")
                .Type<NonNullType<IntType>>();

            descriptor.Field(cl => cl.Longitude)
                .Description("The longitude coordinate of the location")
                .Type<NonNullType<FloatType>>();

            descriptor.Field(cl => cl.Latitude)
                .Description("The latitude coordinate of the location")
                .Type<NonNullType<FloatType>>();

            descriptor.Field(cl => cl.Location)
                .Description("Human-readable address or location description")
                .Type<NonNullType<StringType>>();

            descriptor.Field(cl => cl.Active)
                .Description("Whether this is the civilian's current active location")
                .Type<NonNullType<BooleanType>>();

            // Configure navigation property with resolver
            descriptor.Field(cl => cl.Civilian)
                .Description("The civilian associated with this location")
                .Type<CivilianType>()
                .Resolve(async ctx =>
                {
                    var db = ctx.Service<AppDbContext>();
                    return await db.Civilians
                        .FirstOrDefaultAsync(c => c.Id == ctx.Parent<CivilianLocation>().CivilianId);
                });
        }
    }
}
