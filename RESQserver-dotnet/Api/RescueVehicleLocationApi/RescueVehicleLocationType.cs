using Core.models;
using Core.Models;
using HotChocolate.Types;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using RESQserver_dotnet.Api.RescueVehicleApi;
using System.Threading.Tasks;

namespace RESQserver_dotnet.Api.RescueVehicleLocationApi
{
    public class RescueVehicleLocationType : ObjectType<RescueVehicleLocation>
    {
        protected override void Configure(IObjectTypeDescriptor<RescueVehicleLocation> descriptor)
        {
            descriptor.Description("Represents a rescue vehicle's location in the emergency response system");

            // Configure scalar fields
            descriptor.Field(rvl => rvl.Id)
                .Description("The unique identifier of the location record")
                .Type<NonNullType<IdType>>();

            descriptor.Field(rvl => rvl.RescueVehicleId)
                .Description("The ID of the rescue vehicle this location belongs to")
                .Type<NonNullType<IntType>>();

            descriptor.Field(rvl => rvl.Longitude)
                .Description("The longitude coordinate of the location")
                .Type<NonNullType<FloatType>>();

            descriptor.Field(rvl => rvl.Latitude)
                .Description("The latitude coordinate of the location")
                .Type<NonNullType<FloatType>>();

            descriptor.Field(rvl => rvl.Location)
                .Description("Human-readable address or location description")
                .Type<NonNullType<StringType>>();

            descriptor.Field(rvl => rvl.Active)
                .Description("Whether this is the vehicle's current active location")
                .Type<NonNullType<BooleanType>>();

            // Configure navigation property with resolver
            descriptor.Field(rvl => rvl.RescueVehicle)
                .Description("The rescue vehicle associated with this location")
                .Type<RescueVehicleType>()
                .Resolve(async ctx =>
                {
                    var db = ctx.Service<AppDbContext>();
                    return await db.RescueVehicles
                        .AsNoTracking()
                        .FirstOrDefaultAsync(v => v.Id == ctx.Parent<RescueVehicleLocation>().RescueVehicleId);
                });
        }
    }
}