using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using RESQserver_dotnet.Api.RescueVehicleApi;
using RESQserver_dotnet.Api.RescueVehicleRequestApi;

namespace RESQserver_dotnet.Api.RescueVehicleAssignmentApi
{
    public class RescueVehicleAssignmentType : ObjectType<RescueVehicleAssignment>
    {
        protected override void Configure(IObjectTypeDescriptor<RescueVehicleAssignment> descriptor)
        {
            descriptor.Description("Represents an assignment of a rescue vehicle to an emergency request");

            descriptor.Field(a => a.Id)
                .Description("The unique identifier for the assignment")
                .Type<NonNullType<IdType>>();

            // Timestamp Fields
            descriptor.Field(a => a.Timestamp)
                .Description("When the assignment was initially created")
                .Type<NonNullType<DateTimeType>>();

            descriptor.Field(a => a.ArrivalTime)
                .Description("When the vehicle arrived at the scene (if applicable)")
                .Type<DateTimeType>();

            descriptor.Field(a => a.DepartureTime)
                .Description("When the vehicle departed from the scene (if applicable)")
                .Type<DateTimeType>();

            // Relationships
            descriptor.Field(a => a.RescueVehicleRequest)
                .Description("The emergency request this assignment is for")
                .Type<NonNullType<RescueVehicleRequestType>>()
                .ResolveWith<Resolvers>(r => r.GetRescueVehicleRequest(default!, default!));

            descriptor.Field(a => a.RescueVehicle)
                .Description("The vehicle assigned to this request")
                .Type<NonNullType<RescueVehicleType>>()
                .ResolveWith<Resolvers>(r => r.GetRescueVehicle(default!, default!));

            // Computed Fields
            descriptor.Field("durationMinutes")
                .Description("Total active assignment duration in minutes")
                .Type<IntType>()
                .Resolve(ctx =>
                {
                    var assignment = ctx.Parent<RescueVehicleAssignment>();
                    if (assignment.ArrivalTime == null || assignment.DepartureTime == null)
                        return null;

                    return (assignment.DepartureTime - assignment.ArrivalTime)?.TotalMinutes;
                });
        }

        private class Resolvers
        {
            public async Task<RescueVehicleRequest> GetRescueVehicleRequest(
                [Parent] RescueVehicleAssignment assignment,
                [Service] AppDbContext dbContext)
            {
                return await dbContext.RescueVehicleRequests
                    .FirstOrDefaultAsync(r => r.Id == assignment.RescueVehicleRequestId);
            }

            public async Task<RescueVehicle> GetRescueVehicle(
                [Parent] RescueVehicleAssignment assignment,
                [Service] AppDbContext dbContext)
            {
                return await dbContext.RescueVehicles
                    .FirstOrDefaultAsync(v => v.Id == assignment.RescueVehicleId);
            }
        }
    }
}
