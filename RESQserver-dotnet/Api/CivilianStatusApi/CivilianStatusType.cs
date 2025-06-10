using Core.Models;
using HotChocolate.Types;
using Infrastructure.Data;
using HotChocolate.Resolvers;

public class CivilianStatusType : ObjectType<CivilianStatus>
{
    protected override void Configure(IObjectTypeDescriptor<CivilianStatus> descriptor)
    {
        descriptor.Description("Represents a civilian status (like role or user type).");

        descriptor
            .Field(c => c.Id)
            .Description("The unique identifier of the civilian status.");

        descriptor
            .Field(c => c.Role)
            .Description("The role of the civilian.");

        descriptor
            .Field(c => c.CivilianTypeRequests)
            .Ignore();

        descriptor
            .Field(c => c.Civilians)
            .Ignore();

        
        /*
        descriptor
            .Field("civilians")
            .ResolveWith<Resolvers>(r => r.GetCivilians(default!, default!))
            .UseDbContext<AppDbContext>()
            .Type<ListType<CivilianType>>();
        */
    }
}
