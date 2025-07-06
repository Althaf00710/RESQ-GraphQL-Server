using Core.DTO;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.SnakeApi
{
    [ExtendObjectType<Mutation>]
    public class SnakeMutation
    {
        public async Task<SnakePayload> CreateSnake(SnakeCreateInput input, IFile? image, [Service] ISnakeService service)
        {
            try
            {
                var snake = await service.Add(input, image);
                return new SnakePayload(true, "Snake created successfully", snake);
            }
            catch (Exception ex)
            {
                return new SnakePayload(false, ex.Message);
            }
        }

        public async Task<SnakePayload> UpdateSnake(int id, SnakeUpdateInput input, IFile? image, [Service] ISnakeService service)
        {
            try
            {
                var snake = await service.Update(id, input, image);
                return new SnakePayload(true, "Snake updated successfully", snake);
            }
            catch (Exception ex)
            {
                return new SnakePayload(false, ex.Message);
            }
        }

        public async Task<SnakePayload> DeleteSnake(int id, [Service] ISnakeService service)
        {
            try
            {
                await service.Delete(id);
                return new SnakePayload(true, "Snake deleted successfully");
            }
            catch (Exception ex)
            {
                return new SnakePayload(false, ex.Message);
            }
        }
    }
}
