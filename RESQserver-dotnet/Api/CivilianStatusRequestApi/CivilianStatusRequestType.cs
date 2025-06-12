using Core.Models;
using HotChocolate.Types;
using Infrastructure.Data;

namespace RESQserver_dotnet.Api.CivilianStatusRequestApi
{
    public class CivilianStatusRequestType : ObjectType<CivilianStatusRequest>
    {
        protected override void Configure(IObjectTypeDescriptor<CivilianStatusRequest> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CivilianStatusId).Type<NonNullType<IntType>>();
            descriptor.Field(c => c.CivilianId).Type<NonNullType<IntType>>();
            descriptor.Field(c => c.proofImage).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.status).Type<StringType>(); 
            descriptor.Field(c => c.CreatedAt).Type<NonNullType<DateTimeType>>();
            descriptor.Field(c => c.UpdatedAt).Type<NonNullType<DateTimeType>>();

            // Navigation properties
            descriptor.Field(c => c.CivilianStatus)
                .Type<CivilianStatusType>()  
                .ResolveWith<CivilianStatusResolvers>(r => r.GetCivilianStatus(default!, default!));

            descriptor.Field(c => c.Civilian)
                .Type<CivilianApi.CivilianType>()       
                .ResolveWith<CivilianResolvers>(r => r.GetCivilian(default!, default!));
        }

        private class CivilianStatusResolvers
        {
            public async Task<CivilianStatus> GetCivilianStatus(
                [Parent] CivilianStatusRequest request,
                [Service] AppDbContext dbContext)
            {
                return await dbContext.CivilianStatuses.FindAsync(request.CivilianStatusId);
            }
        }

        private class CivilianResolvers
        {
            public async Task<Civilian> GetCivilian(
                [Parent] CivilianStatusRequest request,
                [Service] AppDbContext dbContext)
            {
                return await dbContext.Civilians.FindAsync(request.CivilianId);
            }
        }
    }
}
