using Core.Models;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;
using RESQserver_dotnet.Api.CivilianType;
using Microsoft.EntityFrameworkCore;
using RESQserver_dotnet.Api.CivilianLocationApi;
using RESQserver_dotnet.Api.CivilianStatusRequestApi;
using RESQserver_dotnet.Api.RescueVehicleRequest;

namespace RESQserver_dotnet.Api.CivilianApi
{
    public class CivilianType : ObjectType<Core.Models.Civilian>
    {
        protected override void Configure(IObjectTypeDescriptor<Core.Models.Civilian> descriptor)
        {
            descriptor.Description("Represents a civilian in the emergency response system.");

            descriptor
                .Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor
                .Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.PhoneNumber)
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.NicNumber)
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.JoinedDate)
                .Type<NonNullType<DateTimeType>>();

            descriptor
                .Field(c => c.CivilianStatusId)
                .Type<NonNullType<IntType>>();

            descriptor.Field("civilianStatus")
                .Type<CivilianStatusType>()
                .Resolve(async ctx =>
                {
                    var db = ctx.Service<AppDbContext>();
                    return await db.CivilianStatuses
                        .FirstOrDefaultAsync(s => s.Id == ctx.Parent<Civilian>().CivilianStatusId);
                });

            //descriptor.Field("civilianLocations")
            //    .Type<ListType<CivilianLocationType>>()
            //    .Resolve(async ctx =>
            //    {
            //        var db = ctx.Service<AppDbContext>();
            //        return await db.CivilianLocations
            //            .Where(l => l.CivilianId == ctx.Parent<Civilian>().Id)
            //            .ToListAsync();
            //    });

            //descriptor.Field("rescueVehicleRequests")
            //    .Type<ListType<RescueVehicleRequestType>>()
            //    .Resolve(async ctx =>
            //    {
            //        var db = ctx.Service<AppDbContext>();
            //        return await db.RescueVehicleRequests
            //            .Where(r => r.CivilianId == ctx.Parent<Civilian>().Id)
            //            .ToListAsync();
            //    });

            //descriptor.Field("civilianTypeRequests")
            //    .Type<ListType<CivilianStatusRequestType>>()
            //    .Resolve(async ctx =>
            //    {
            //        var db = ctx.Service<AppDbContext>();
            //        return await db.CivilianStatusRequests
            //            .Where(r => r.CivilianId == ctx.Parent<Civilian>().Id)
            //            .ToListAsync();
            //    });
        }
    }
}
