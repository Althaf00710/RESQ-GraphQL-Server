using Core.Models;
using HotChocolate.Types;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using RESQserver_dotnet.Api.EmergencySubCategoryApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RESQserver_dotnet.Api.EmergencyCategoryApi
{
    public class EmergencyCategoryType : ObjectType<EmergencyCategory>
    {
        protected override void Configure(IObjectTypeDescriptor<EmergencyCategory> descriptor)
        {
            descriptor.Description("Represents a category of emergency situations");

            // Configure scalar fields
            descriptor.Field(ec => ec.Id)
                .Description("The unique identifier of the emergency category")
                .Type<NonNullType<IdType>>();

            descriptor.Field(ec => ec.Name)
                .Description("The name of the emergency category")
                .Type<NonNullType<StringType>>();

            descriptor.Field(ec => ec.Description)
                .Description("Detailed description of the emergency category")
                .Type<StringType>();

            descriptor.Field(ec => ec.Icon)
                .Description("URL or identifier for the category's icon")
                .Type<StringType>();

            //// Configure navigation properties with resolvers
            //descriptor.Field(ec => ec.EmergencyToVehicles)
            //    .Description("Vehicle types suitable for this emergency category")
            //    .Type<ListType<EmergencyToVehicleType>>()
            //    .Resolve(async ctx =>
            //    {
            //        var db = ctx.Service<AppDbContext>();
            //        return await db.EmergencyToVehicles
            //            .AsNoTracking()
            //            .Where(etv => etv.EmergencyCategoryId == ctx.Parent<EmergencyCategory>().Id)
            //            .ToListAsync();
            //    });

            //descriptor.Field(ec => ec.EmergencyToCivilians)
            //    .Description("Civilians associated with this emergency category")
            //    .Type<ListType<EmergencyToCivilianType>>()
            //    .Resolve(async ctx =>
            //    {
            //        var db = ctx.Service<AppDbContext>();
            //        return await db.EmergencyToCivilians
            //            .AsNoTracking()
            //            .Where(etc => etc.EmergencyCategoryId == ctx.Parent<EmergencyCategory>().Id)
            //            .ToListAsync();
            //    });

            //// Add this if you have EmergencySubCategories relationship
            descriptor.Field(ec => ec.EmergencySubCategories)
                .Description("Subcategories under this emergency category")
                .Type<ListType<EmergencySubCategoryType>>()
                .Resolve(async ctx =>
                {
                    var db = ctx.Service<AppDbContext>();
                    return await db.EmergencySubCategories
                        .AsNoTracking()
                        .Where(esc => esc.EmergencyCategoryId == ctx.Parent<EmergencyCategory>().Id)
                        .ToListAsync();
                });
        }
    }
}