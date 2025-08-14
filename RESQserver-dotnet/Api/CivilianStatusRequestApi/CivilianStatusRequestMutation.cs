using Core.DTO;
using Core.Services.Interfaces;
using RESQserver_dotnet.Api.CivilianApi;
using RESQserver_dotnet.Api.UserApi;

namespace RESQserver_dotnet.Api.CivilianStatusRequestApi
{
    [ExtendObjectType<Mutation>]
    public class CivilianStatusRequestMutation
    {
        public async Task<CivilianStatusRequestPayload> CreateCivilianStatusRequest(
            CivilianStatusRequestCreateInput input,
            IFile proofPicture,
            [Service] ICivilianStatusRequestService service)
        {
            try
            {
                var created = await service.Add(input, proofPicture);
                return new CivilianStatusRequestPayload(true, "Civilian Status Request created successfully", created);
            }
            catch (Exception ex)
            {
                return new CivilianStatusRequestPayload(false, $"Failed to create Civilian Status Request: {ex.Message}");
            }
        }

        public async Task<CivilianStatusRequestPayload> UpdateCivilianStatusRequest(int id, CivilianStatusRequestUpdateInput input, [Service] ICivilianStatusRequestService service)
        {
            try
            {
                var request = await service.Update(id, input);
                return new CivilianStatusRequestPayload(true, "Civilian Status Request updated successfully", request);
            }
            catch (Exception ex)
            {
                return new CivilianStatusRequestPayload(false, ex.Message);
            }



        }
    }
}
