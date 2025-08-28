using Core.DTO;
using Core.Models;
using Core.Services.Interfaces;
using HotChocolate;
using System;
using System.Threading.Tasks;

namespace RESQserver_dotnet.Api.RescueVehicleRequestApi
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class RescueVehicleRequestMutation
    {
        public async Task<RescueVehicleRequestPayload> CreateRescueVehicleRequest(
            RescueVehicleRequestCreateInput input, 
            IFile? proofImage,
            [Service] IRescueVehicleRequestService service)
        {
            try
            {
                var request = await service.Add(input, proofImage);
                return new RescueVehicleRequestPayload(true, "Rescue vehicle request created successfully", request);
            }
            catch (ArgumentNullException ex)
            {
                return new RescueVehicleRequestPayload(false, $"Invalid input: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return new RescueVehicleRequestPayload(false, ex.Message);
            }
            catch (Exception ex)
            {
                return new RescueVehicleRequestPayload(false, ex.Message);
            }
        }

        public async Task<RescueVehicleRequestPayload> UpdateRescueVehicleRequest(
            int id,
            RescueVehicleRequestUpdateInput input,
            [Service] IRescueVehicleRequestService service)
        {
            try
            {
                var request = await service.Update(id, input);
                return new RescueVehicleRequestPayload(true, "Rescue vehicle request updated successfully", request);
            }
            catch (InvalidOperationException ex)
            {
                return new RescueVehicleRequestPayload(false, ex.Message);
            }
            catch (Exception ex)
            {
                return new RescueVehicleRequestPayload(false, ex.Message);
            }
        }

        public async Task<RescueVehicleRequestPayload> DeleteRescueVehicleRequest(int id, [Service] IRescueVehicleRequestService service)
        {
            try
            {
                await service.Delete(id);
                return new RescueVehicleRequestPayload(true, "Rescue vehicle request deleted successfully");
            }
            catch (Exception ex)
            {
                return new RescueVehicleRequestPayload(false, ex.Message);
            }
        }
    }
}