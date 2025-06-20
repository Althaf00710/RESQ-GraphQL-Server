using Core.Models;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.FirstAidDetailApi
{
    [ExtendObjectType<Query>]
    public class FirstAidDetailQuery
    {
        public async Task<FirstAidDetailPayload> GetFirstAidDetailById(int id, [Service] IFirstAidDetailService firstAidDetailService)
        {
            try
            {
                var firstAidDetail = await firstAidDetailService.GetByIdAsync(id);
                if (firstAidDetail == null)
                {
                    return new FirstAidDetailPayload(false, "First Aid Detail not found");
                }
                return new FirstAidDetailPayload(true, "First Aid Detail Found", firstAidDetail);
            }
            catch (Exception ex)
            {
                return new FirstAidDetailPayload(false, ex.Message);
            }
        }

        public async Task<IEnumerable<FirstAidDetail>> GetFirstAidDetails([Service] IFirstAidDetailService firstAidDetailService)
        {
            return await firstAidDetailService.GetAllAsync();
        }

        public async Task<IEnumerable<FirstAidDetail>> GetFirstAidDetailsBySubCategoryId(int emergencySubCategoryId, [Service] IFirstAidDetailService firstAidDetailService)
        {
            return await firstAidDetailService.GetBySubCategoryIdAsync(emergencySubCategoryId);
        }
    }
}
