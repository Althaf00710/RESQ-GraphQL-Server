using Core.DTO;
using Core.Models;
using Core.Services.Interfaces;
using RESQserver_dotnet.Api.CivilianType;

namespace RESQserver_dotnet.Api.CivilianStatusApi
{
    [ExtendObjectType<Mutation>]
    public class CivilianStatusMutation
    {
        public async Task<CivilianStatusPayload> CreateCivilianStatus(CivilianStatusCreateInput input, [Service] ICivilianStatusService service)
        {
            try
            {
                var created = await service.Add(input);
                return new CivilianStatusPayload(true, "Civilian status created successfully", created);
            }
            catch (Exception ex)
            {
                return new CivilianStatusPayload(false, $"Failed to create civilian status: {ex.Message}");
            }
        }

        public async Task<CivilianStatusPayload> UpdateCivilianStatus(
            int id,
            CivilianStatusUpdateInput input,
            [Service] ICivilianStatusService service)
        {
            try
            {
                var updated = await service.Update(id, input);
                return new CivilianStatusPayload(true, "Civilian status updated successfully", updated);
            }
            catch (Exception ex)
            {
                return new CivilianStatusPayload(false, $"Failed to update civilian status: {ex.Message}");
            }
        }

        public async Task<CivilianStatusPayload> DeleteCivilianStatus(
            int id,
            [Service] ICivilianStatusService service)
        {
            try
            {
                var deleted = await service.Delete(id);
                if (!deleted)
                    return new CivilianStatusPayload(false, $"Civilian status with ID {id} not found");

                return new CivilianStatusPayload(true, "Civilian status deleted successfully");
            }
            catch (Exception ex)
            {
                return new CivilianStatusPayload(false, $"Failed to delete civilian status: {ex.Message}");
            }
        }
    }
}
