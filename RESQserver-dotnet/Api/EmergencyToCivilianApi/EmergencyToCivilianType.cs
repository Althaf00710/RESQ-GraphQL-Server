using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using RESQserver_dotnet.Api.EmergencyCategoryApi;

namespace RESQserver_dotnet.Api.EmergencyToCivilianApi
{
    public class EmergencyToCivilianType : ObjectType<EmergencyToCivilian>
    {
        protected override void Configure(IObjectTypeDescriptor<EmergencyToCivilian> descriptor)
        {
            descriptor.Description("Defines which civilian statuses should be alerted for specific emergency categories");

            // Scalar fields
            descriptor.Field(etc => etc.Id)
                .Description("The unique identifier")
                .Type<NonNullType<IdType>>();

            descriptor.Field(etc => etc.EmergencyCategoryId)
                .Description("The emergency category ID")
                .Type<NonNullType<IntType>>();

            descriptor.Field(etc => etc.CivilianStatusId)
                .Description("The civilian status ID that should be alerted")
                .Type<NonNullType<IntType>>();

            // Navigation properties
            descriptor.Field(etc => etc.EmergencyCategory)
                .Description("The emergency category")
                .Type<EmergencyCategoryType>()
                .Resolve(async ctx =>
                {
                    var db = ctx.Service<AppDbContext>();
                    return await db.EmergencyCategories
                        .AsNoTracking()
                        .FirstOrDefaultAsync(ec => ec.Id == ctx.Parent<EmergencyToCivilian>().EmergencyCategoryId);
                });

            descriptor.Field(etc => etc.CivilianStatus)
                .Description("The civilian status to alert")
                .Type<CivilianStatusType>()
                .Resolve(async ctx =>
                {
                    var db = ctx.Service<AppDbContext>();
                    return await db.CivilianStatuses
                        .AsNoTracking()
                        .FirstOrDefaultAsync(cs => cs.Id == ctx.Parent<EmergencyToCivilian>().CivilianStatusId);
                });
        }
    }
}
