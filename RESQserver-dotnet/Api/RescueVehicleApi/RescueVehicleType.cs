
using Core.Models;
using HotChocolate.Types;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using RESQserver_dotnet.Api.RescueVehicleAssignmentApi;
using RESQserver_dotnet.Api.RescueVehicleCategoryApi;
using System.Threading.Tasks;

namespace RESQserver_dotnet.Api.RescueVehicleApi
{
    public class RescueVehicleType : ObjectType<RescueVehicle>
    {
        protected override void Configure(IObjectTypeDescriptor<RescueVehicle> descriptor)
        {
            descriptor.Description("Represents a rescue vehicle in the emergency response system");

            descriptor.Field(r => r.Id)
                .Description("The unique identifier of the rescue vehicle")
                .Type<NonNullType<IdType>>();

            descriptor.Field(r => r.Code)
                .Description("The unique code of the rescue vehicle (e.g., A1000, F1001)")
                .Type<NonNullType<StringType>>();

            descriptor.Field(r => r.PlateNumber)
                .Description("The license plate number of the vehicle")
                .Type<NonNullType<StringType>>();

            descriptor.Field(r => r.Status)
                .Description("The current status of the vehicle (Active, On-Duty, Inactive)")
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
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.Id == ctx.Parent<RescueVehicle>().RescueVehicleCategoryId);
                });

            descriptor.Field(r => r.Password).Ignore();

            //descriptor.Field(r => r.RescueVehicleLocations)
            //    .Description("Location history of this vehicle")
            //    .Type<ListType<RescueVehicleLocationType>>()
            //    .Resolve(async ctx =>
            //    {
            //        var db = ctx.Service<AppDbContext>();
            //        return await db.RescueVehicleLocations
            //            .AsNoTracking()
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
            //            .AsNoTracking()
            //            .Where(a => a.RescueVehicleId == ctx.Parent<RescueVehicle>().Id)
            //            .OrderByDescending(a => a.AssignmentTime)
            //            .ToListAsync();
            //    });
        }
    }
}