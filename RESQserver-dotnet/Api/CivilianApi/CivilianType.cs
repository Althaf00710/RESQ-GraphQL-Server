using Core.Models;
using HotChocolate.Types;
using Infrastructure.Data;
using RESQserver_dotnet.Api.CivilianType;

namespace RESQserver_dotnet.Api.Civilian
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

            descriptor
                .Field(c => c.CivilianStatus)
                .Type<CivilianStatusType>() // Reference to nested type  
                .ResolveWith<Resolvers>(r => r.GetCivilianType(default!, default!))
                .UseDbContext<AppDbContext>(); 

            descriptor
                .Field(c => c.CivilianLocations)
                .Ignore(); // Optional  

            descriptor
                .Field(c => c.RescueVehicleRequests)
                .Ignore();

            descriptor
                .Field(c => c.CivilianTypeRequests)
                .Ignore();
        }

        private class Resolvers
        {
            public Core.Models.CivilianStatus GetCivilianType(Core.Models.Civilian civilian, [ScopedService] AppDbContext context)
            {
                return context.CivilianTypes.FirstOrDefault(ct => ct.Id == civilian.CivilianStatusId)!;
            }
        }
    }
}
