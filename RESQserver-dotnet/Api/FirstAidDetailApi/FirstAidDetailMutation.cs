using Core.DTO;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.FirstAidDetailApi
{
    [ExtendObjectType<Mutation>]
    public class FirstAidDetailMutation
    {
        public async Task<FirstAidDetailPayload> CreateFirstAidDetail(FirstAidDetailCreateInput firstAidDetail, IFile? image, [Service] IFirstAidDetailService service)
        {
            try
            {
                var createdDetail = await service.Add(firstAidDetail, image);
                return new FirstAidDetailPayload(true, "First Aid Detail created successfully", createdDetail);
            }
            catch (Exception ex)
            {
                return new FirstAidDetailPayload(false, ex.Message);
            }
        }
        public async Task<FirstAidDetailPayload> UpdateFirstAidDetail(int id, FirstAidDetailUpdateInput firstAidDetail, IFile? image, [Service] IFirstAidDetailService firstAidDetailService)
        {
            try
            {
                var updatedDetail = await firstAidDetailService.Update(id, firstAidDetail, image);
                return new FirstAidDetailPayload(true, "First Aid Detail updated successfully", updatedDetail);
            }
            catch (Exception ex)
            {
                return new FirstAidDetailPayload(false, ex.Message);
            }
        }
        public async Task<FirstAidDetailPayload> DeleteFirstAidDetail(int id, [Service] IFirstAidDetailService firstAidDetailService)
        {
            try
            {
                await firstAidDetailService.Delete(id);
                return new FirstAidDetailPayload(true, "First Aid Detail deleted successfully");
            }
            catch (Exception ex)
            {
                return new FirstAidDetailPayload(false, ex.Message);
            }
        }
    }
}
