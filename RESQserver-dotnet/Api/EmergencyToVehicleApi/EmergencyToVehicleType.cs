using Core.Models;
using HotChocolate.Types;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using RESQserver_dotnet.Api.EmergencyCategoryApi;
using RESQserver_dotnet.Api.RescueVehicleCategoryApi;
using System.Threading.Tasks;

namespace RESQserver_dotnet.Api.EmergencyToVehicleApi
{
    public class EmergencyToVehicleType : ObjectType<EmergencyToVehicle>
    {
        protected override void Configure(IObjectTypeDescriptor<EmergencyToVehicle> descriptor)
        {
            descriptor.Description("Defines which vehicle categories should be dispatched for specific emergency categories");

            descriptor.Field(etv => etv.Id)
                .Description("The unique identifier")
                .Type<NonNullType<IdType>>();

            descriptor.Field(etv => etv.EmergencyCategoryId)
                .Description("The emergency category ID")
                .Type<NonNullType<IntType>>();

            descriptor.Field(etv => etv.VehicleCategoryId)
                .Description("The vehicle category ID that should be dispatched")
                .Type<NonNullType<IntType>>();

            descriptor.Field(etv => etv.EmergencyCategory)
                .Description("The emergency category")
                .Type<EmergencyCategoryType>()
                .Resolve(async ctx =>
                {
                    var db = ctx.Service<AppDbContext>();
                    return await db.EmergencyCategories
                        .AsNoTracking()
                        .FirstOrDefaultAsync(ec => ec.Id == ctx.Parent<EmergencyToVehicle>().EmergencyCategoryId);
                });

            descriptor.Field(etv => etv.RescueVehicleCategory)
                .Description("The vehicle category to dispatch")
                .Type<RescueVehicleCategoryType>()
                .Resolve(async ctx =>
                {
                    var db = ctx.Service<AppDbContext>();
                    return await db.RescueVehicleCategories
                        .AsNoTracking()
                        .FirstOrDefaultAsync(vc => vc.Id == ctx.Parent<EmergencyToVehicle>().VehicleCategoryId);
                });
        }
    }
}