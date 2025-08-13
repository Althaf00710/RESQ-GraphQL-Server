using Core.Models;
using HotChocolate.Types;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using RESQserver_dotnet.Api.RescueVehicleApi;

namespace RESQserver_dotnet.Api.RescueVehicleLocationApi
{
    public class RescueVehicleLocationType : ObjectType<RescueVehicleLocation>
    {
        protected override void Configure(IObjectTypeDescriptor<RescueVehicleLocation> descriptor)
        {
            descriptor.Description("Represents a rescue vehicle's geo‐location and address.");

            descriptor.Ignore(rvl => rvl.Location);

            descriptor.Field(rvl => rvl.Id)
                .Type<NonNullType<IdType>>()
                .Description("Primary key of the location record.");

            descriptor.Field(rvl => rvl.RescueVehicleId)
                .Type<NonNullType<IntType>>()
                .Description("FK to the rescue vehicle.");

            descriptor.Field(rvl => rvl.Address)
                .Type<NonNullType<StringType>>()
                .Description("Human‐readable address or description.");

            descriptor.Field(rvl => rvl.Active)
                .Type<NonNullType<BooleanType>>()
                .Description("Whether this is the vehicle's current active location.");

            descriptor.Field(rvl => rvl.LastActive)
                .Type<NonNullType<DateTimeType>>()
                .Description("UTC timestamp when this location was last active.");

            // Expose the Point as latitude & longitude
            descriptor.Field("latitude")
                .Type<NonNullType<FloatType>>()
                .Description("Latitude (Y) of the Point.")
                .Resolve(ctx => ((Point)ctx.Parent<RescueVehicleLocation>().Location).Y);

            descriptor.Field("longitude")
                .Type<NonNullType<FloatType>>()
                .Description("Longitude (X) of the Point.")
                .Resolve(ctx => ((Point)ctx.Parent<RescueVehicleLocation>().Location).X);

            // Navigation
            descriptor.Field(rvl => rvl.RescueVehicle)
                .Type<NonNullType<RescueVehicleType>>()
                .Description("The associated rescue vehicle.")
                .ResolveWith<Resolvers>(r => r.GetVehicleAsync(default!, default!));
        }

        private class Resolvers
        {
            public async Task<RescueVehicle> GetVehicleAsync(
                [Parent] RescueVehicleLocation loc,
                [Service] AppDbContext db)
            {
                return await db.RescueVehicles
                    .AsNoTracking()
                    .FirstAsync(rv => rv.Id == loc.RescueVehicleId);
            }
        }
    }
}
