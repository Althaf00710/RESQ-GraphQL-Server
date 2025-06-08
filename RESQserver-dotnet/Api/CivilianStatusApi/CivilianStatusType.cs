using Core.Models;

namespace RESQserver_dotnet.Api.CivilianType
{
    public class CivilianStatusType : ObjectType<CivilianStatus>
    {
        protected override void Configure(IObjectTypeDescriptor<CivilianStatus> descriptor)
        {
            descriptor.Description("Represents a role of a civilian, such as Doctor or Traffic Police.");

            descriptor
                .Field(ct => ct.Id)
                .Type<NonNullType<IdType>>();

            descriptor
                .Field(ct => ct.Role)
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(ct => ct.Civilians)
                .Ignore(); // To avoid circular 
        }
    }
}
