using Core.DTO;
using Core.Services.Interfaces;
using RESQserver_dotnet.Api.CivilianType;

namespace RESQserver_dotnet.Api.CivilianApi
{
    [ExtendObjectType<Mutation>]
    public class CivilianMutation
    {
        public async Task<CivilianPayload> CreateCivilian(CivilianCreateInput input, [Service] ICivilianService service)
        {
            try
            {
                var created = await service.Add(input);
                return new CivilianPayload(true, "Civilian created successfully", created);
            }
            catch (Exception ex)
            {
                return new CivilianPayload(false, $"Failed to create civilian: {ex.Message}");
            }
        }

        public async Task<CivilianPayload> UpdateCivilian(
            int id,
            CivilianUpdateInput input,
            [Service] ICivilianService service)
        {
            try
            {
                var updated = await service.Update(id, input);
                return new CivilianPayload(true, "Civilian updated successfully", updated);
            }
            catch (Exception ex)
            {
                return new CivilianPayload(false, $"Failed to update civilian: {ex.Message}");
            }
        }

        public async Task<CivilianPayload> DeleteCivilianStatus(
            int id,
            [Service] ICivilianService service)
        {
            try
            {
                var deleted = await service.Delete(id);
                if (!deleted)
                    return new CivilianPayload(false, $"Civilian with ID {id} not found");

                return new CivilianPayload(true, "Civilian deleted successfully");
            }
            catch (Exception ex)
            {
                return new CivilianPayload(false, $"Failed to delete civilian: {ex.Message}");
            }
        }
    }
}
