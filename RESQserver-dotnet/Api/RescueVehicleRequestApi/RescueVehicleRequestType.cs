using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using RESQserver_dotnet.Api.CivilianApi;
using RESQserver_dotnet.Api.EmergencyCategoryApi;
using RESQserver_dotnet.Api.RescueVehicleAssignmentApi;

namespace RESQserver_dotnet.Api.RescueVehicleRequestApi
{
    public class RescueVehicleRequestType : ObjectType<RescueVehicleRequest>
    {
        protected override void Configure(IObjectTypeDescriptor<RescueVehicleRequest> descriptor)
        {
            descriptor.Description("Represents a request for rescue vehicle assistance");

            // ID Field
            descriptor.Field(r => r.Id)
                .Description("The unique identifier for the request")
                .Type<NonNullType<IdType>>();

            // Status Field
            descriptor.Field(r => r.Status)
                .Description("Current status of the request (Searching, Dispatched, InProgress, etc.)")
                .Type<NonNullType<StringType>>();

            // Location Fields
            descriptor.Field(r => r.Location)
                .Description("Human-readable location description")
                .Type<NonNullType<StringType>>();

            descriptor.Field(r => r.Longitude)
                .Description("Geographic longitude coordinate")
                .Type<NonNullType<FloatType>>();

            descriptor.Field(r => r.Latitude)
                .Description("Geographic latitude coordinate")
                .Type<NonNullType<FloatType>>();

            // Timestamp Field
            descriptor.Field(r => r.CreatedAt)
                .Description("When the request was created")
                .Type<NonNullType<DateTimeType>>();

            // Optional Fields
            descriptor.Field(r => r.Description)
                .Description("Additional details about the emergency")
                .Type<StringType>();

            descriptor.Field(r => r.ProofImageURL)
                .Description("URL to supporting image evidence")
                .Type<StringType>();

            // Relationships
            descriptor.Field(r => r.EmergencyCategory)
                .Description("The category of emergency")
                .Type<EmergencyCategoryType>()
                .ResolveWith<Resolvers>(r => r.GetEmergencyCategory(default!, default!));

            descriptor.Field(r => r.Civilian)
                .Description("The civilian who made the request")
                .Type<CivilianType>()
                .ResolveWith<Resolvers>(r => r.GetCivilian(default!, default!));

            //descriptor.Field(r => r.RescueVehicleAssignments)
            //    .Description("List of vehicles assigned to this request")
            //    .Type<ListType<RescueVehicleAssignmentType>>()
            //    .ResolveWith<Resolvers>(r => r.GetAssignments(default!, default!));

            // Computed Field Example
            descriptor.Field("isActive")
                .Description("Whether the request is still active (not completed/cancelled)")
                .Type<NonNullType<BooleanType>>()
                .Resolve(ctx =>
                    ctx.Parent<RescueVehicleRequest>().Status is not ("Completed" or "Failed" or "Cancelled"));
        }

        private class Resolvers
        {
            public async Task<EmergencyCategory> GetEmergencyCategory(
                [Parent] RescueVehicleRequest request,
                [Service] AppDbContext dbContext)
            {
                return await dbContext.EmergencyCategories
                    .FirstOrDefaultAsync(e => e.Id == request.EmergencyCategoryId);
            }

            public async Task<Civilian> GetCivilian(
                [Parent] RescueVehicleRequest request,
                [Service] AppDbContext dbContext)
            {
                return await dbContext.Civilians
                    .FirstOrDefaultAsync(c => c.Id == request.CivilianId);
            }

            public async Task<IEnumerable<RescueVehicleAssignment>> GetAssignments(
                [Parent] RescueVehicleRequest request,
                [Service] AppDbContext dbContext)
            {
                return await dbContext.RescueVehicleAssignments
                    .Where(a => a.RescueVehicleRequestId == request.Id)
                    .ToListAsync();
            }
        }
    }
}
