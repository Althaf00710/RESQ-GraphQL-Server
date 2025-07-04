﻿using Core.Models;
using HotChocolate.Types;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using RESQserver_dotnet.Api.RescueVehicleApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RESQserver_dotnet.Api.RescueVehicleCategoryApi
{
    public class RescueVehicleCategoryType : ObjectType<RescueVehicleCategory>
    {
        protected override void Configure(IObjectTypeDescriptor<RescueVehicleCategory> descriptor)
        {
            descriptor.Description("Represents a category/type of rescue vehicle (e.g., Ambulance, Fire Truck, Police Car)");

            descriptor.Field(c => c.Id)
                .Description("The unique identifier for the vehicle category")
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Name)
                .Description("The name of the vehicle category")
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.RescueVehicles)
                .Description("List of vehicles belonging to this category")
                .Type<ListType<RescueVehicleType>>()
                .Resolve(async ctx =>
                {
                    var db = ctx.Service<AppDbContext>();
                    return await db.RescueVehicles
                        .Where(v => v.RescueVehicleCategoryId == ctx.Parent<RescueVehicleCategory>().Id)
                        .ToListAsync();
                });
        }
    }
}