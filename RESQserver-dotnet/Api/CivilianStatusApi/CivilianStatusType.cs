using Core.Models;
using HotChocolate.Types;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using RESQserver_dotnet.Api.CivilianApi;
using RESQserver_dotnet.Api.CivilianStatusRequestApi;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CivilianStatusType : ObjectType<CivilianStatus>
{
    protected override void Configure(IObjectTypeDescriptor<CivilianStatus> descriptor)
    {
        descriptor.Description("Represents a civilian status (like role or user type).");

        // Configure scalar fields
        descriptor.Field(c => c.Id)
            .Description("The unique identifier of the civilian status.")
            .Type<NonNullType<IntType>>();

        descriptor.Field(c => c.Role)
            .Description("The role of the civilian.")
            .Type<NonNullType<StringType>>();

        // Configure navigation properties with resolvers
        descriptor.Field(c => c.Civilians)
            .Description("List of civilians with this status")
            .Type<ListType<CivilianType>>()
            .Resolve(async ctx =>
            {
                var db = ctx.Service<AppDbContext>();
                return await db.Civilians
                    .Where(c => c.CivilianStatusId == ctx.Parent<CivilianStatus>().Id)
                    .ToListAsync();
            });

        descriptor.Field(c => c.CivilianStatusRequests)
            .Description("Status change requests for this civilian type")
            .Type<ListType<CivilianStatusRequestType>>()
            .Resolve(async ctx =>
            {
                var db = ctx.Service<AppDbContext>();
                return await db.CivilianStatusRequests
                    .Where(r => r.CivilianStatusId == ctx.Parent<CivilianStatus>().Id)
                    .ToListAsync();
            });

        descriptor.Field(c => c.EmergencyToCivilians)
            .Name("emergencyToCivilians")
            .Description("Which emergency categories this status is allowed for")
            .Type<NonNullType<ListType<NonNullType<ObjectType<EmergencyToCivilian>>>>>()
            .Resolve(async ctx =>
            {
                var db = ctx.Service<AppDbContext>();
                var statusId = ctx.Parent<CivilianStatus>().Id;
                return await db.EmergencyToCivilians
                    .Include(link => link.EmergencyCategory)
                    .Where(link => link.CivilianStatusId == statusId)
                    .ToListAsync();
            });
    }

    // Alternative resolver class approach (optional)
    private class Resolvers
    {
        public async Task<List<Civilian>> GetCivilians(
            [Parent] CivilianStatus status,
            [Service] AppDbContext dbContext)
        {
            return await dbContext.Civilians
                .Where(c => c.CivilianStatusId == status.Id)
                .ToListAsync();
        }

        public async Task<List<CivilianStatusRequest>> GetCivilianTypeRequests(
            [Parent] CivilianStatus status,
            [Service] AppDbContext dbContext)
        {
            return await dbContext.CivilianStatusRequests
                .Where(r => r.CivilianStatusId == status.Id)
                .ToListAsync();
        }
    }
}