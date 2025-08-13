using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace RESQserver_dotnet.Api.RescueVehicleRequestApi
{
    public class RescueVehicleRequestType : ObjectType<RescueVehicleRequest>
    {
        protected override void Configure(IObjectTypeDescriptor<RescueVehicleRequest> descriptor)
        {
            descriptor.Description("Represents a request for rescue vehicle assistance");

            // ID
            descriptor.Field(r => r.Id)
                .Description("The unique identifier for the request")
                .Type<NonNullType<IdType>>();

            // Status
            descriptor.Field(r => r.Status)
                .Description("Current status of the request (Searching, Dispatched, InProgress, etc.)")
                .Type<NonNullType<StringType>>();

            // Address (human-readable)
            descriptor.Field(r => r.Address)
                .Description("Human-readable address or place description")
                .Type<NonNullType<StringType>>();

            // Latitude / Longitude derived from NetTopologySuite Point (SRID 4326)
            descriptor.Field("longitude")
                .Description("Geographic longitude coordinate (from Location.X)")
                .Type<NonNullType<FloatType>>()
                .Resolve(ctx => ctx.Parent<RescueVehicleRequest>().Location.X);

            descriptor.Field("latitude")
                .Description("Geographic latitude coordinate (from Location.Y)")
                .Type<NonNullType<FloatType>>()
                .Resolve(ctx => ctx.Parent<RescueVehicleRequest>().Location.Y);

            // (Optional) Raw WKT for debugging/interop
            descriptor.Field("locationWkt")
                .Description("WKT representation of the location point (SRID 4326)")
                .Type<NonNullType<StringType>>()
                .Resolve(ctx => ctx.Parent<RescueVehicleRequest>().Location.AsText());

            // Timestamps
            descriptor.Field(r => r.CreatedAt)
                .Description("When the request was created")
                .Type<NonNullType<DateTimeType>>();

            // Optional fields
            descriptor.Field(r => r.Description)
                .Description("Additional details about the emergency")
                .Type<StringType>();

            descriptor.Field(r => r.ProofImageURL)
                .Description("URL to supporting image evidence")
                .Type<StringType>();

            // Relationships
            descriptor.Field(r => r.EmergencyCategory)
                .Description("The category of emergency")
                .Type<EmergencyCategoryApi.EmergencyCategoryType>()
                .ResolveWith<Resolvers>(r => r.GetEmergencyCategory(default!, default!));

            descriptor.Field(r => r.Civilian)
                .Description("The civilian who made the request")
                .Type<CivilianApi.CivilianType>()
                .ResolveWith<Resolvers>(r => r.GetCivilian(default!, default!));

            // If you later want to expose assignments, uncomment and keep as-is.
            // descriptor.Field("rescueVehicleAssignments")
            //     .Description("List of vehicles assigned to this request")
            //     .Type<ListType<RescueVehicleAssignmentApi.RescueVehicleAssignmentType>>()
            //     .ResolveWith<Resolvers>(r => r.GetAssignments(default!, default!));

            // Computed field
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
                [Service] AppDbContext dbContext) =>
                await dbContext.EmergencyCategories
                    .FirstOrDefaultAsync(e => e.Id == request.EmergencyCategoryId);

            public async Task<Civilian> GetCivilian(
                [Parent] RescueVehicleRequest request,
                [Service] AppDbContext dbContext) =>
                await dbContext.Civilians
                    .FirstOrDefaultAsync(c => c.Id == request.CivilianId);

            public async Task<IEnumerable<RescueVehicleAssignment>> GetAssignments(
                [Parent] RescueVehicleRequest request,
                [Service] AppDbContext dbContext) =>
                await dbContext.RescueVehicleAssignments
                    .Where(a => a.RescueVehicleRequestId == request.Id)
                    .ToListAsync();
        }
    }
}
