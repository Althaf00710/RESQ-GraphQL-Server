using Core.models;
using Core.Models;
using HotChocolate.Types;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using RESQserver_dotnet.Api.RescueVehicleAssignment;
using RESQserver_dotnet.Api.RescueVehicleCategoryApi;
using RESQserver_dotnet.Api.RescueVehicleLocation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RESQserver_dotnet.Api.RescueVehicleApi
{
    public class RescueVehicleType : ObjectType<RescueVehicle>
    {
        protected override void Configure(IObjectTypeDescriptor<RescueVehicle> descriptor)
        {
            descriptor.Description("Represents a rescue vehicle in the emergency response system");

            // Configure scalar fields
            descriptor.Field(r => r.Id)
                .Description("The unique identifier of the rescue vehicle")
                .Type<NonNullType<IdType>>();

            descriptor.Field(r => r.Code)
                .Description("The unique code of the rescue vehicle")
                .Type<NonNullType<StringType>>();

            descriptor.Field(r => r.PlateNumber)
                .Description("The license plate number of the vehicle")
                .Type<NonNullType<StringType>>();

            descriptor.Field(r => r.Status)
                .Description("The current status of the vehicle (Available, On-Duty, Maintenance)")
                .Type<StringType>();

            descriptor.Field(r => r.RescueVehicleCategoryId)
                .Description("The category ID this vehicle belongs to")
                .Type<NonNullType<IntType>>();

            // Configure navigation properties with resolvers
            descriptor.Field(r => r.RescueVehicleCategory)
                .Description("The category of this rescue vehicle")
                .Type<RescueVehicleCategoryType>()
                .Resolve(async ctx =>
                {
                    var db = ctx.Service<AppDbContext>();
                    return await db.RescueVehicleCategories
                        .FirstOrDefaultAsync(c => c.Id == ctx.Parent<RescueVehicle>().RescueVehicleCategoryId);
                });

            //descriptor.Field(r => r.RescueVehicleLocations)
            //    .Description("Location history of this vehicle")
            //    .Type<ListType<RescueVehicleLocationType>>()
            //    .Resolve(async ctx =>
            //    {
            //        var db = ctx.Service<AppDbContext>();
            //        return await db.RescueVehicleLocations
            //            .Where(l => l.RescueVehicleId == ctx.Parent<RescueVehicle>().Id)
            //            .OrderByDescending(l => l.Timestamp)
            //            .ToListAsync();
            //    });

            //descriptor.Field(r => r.RescueVehicleAssignment)
            //    .Description("Assignment history of this vehicle")
            //    .Type<ListType<RescueVehicleAssignmentType>>()
            //    .Resolve(async ctx =>
            //    {
            //        var db = ctx.Service<AppDbContext>();
            //        return await db.RescueVehicleAssignments
            //            .Where(a => a.RescueVehicleId == ctx.Parent<RescueVehicle>().Id)
            //            .OrderByDescending(a => a.AssignmentTime)
            //            .ToListAsync();
            //    });

        }
    }
}