using Core.Models;
using Core.Repositories.Interfaces;

namespace RESQserver_dotnet.Api.SnakeApi
{
    [ExtendObjectType<Query>]
    public class SnakeQuery
    {
        public async Task<IEnumerable<Snake>> GetSnakes([Service] ISnakeRepository snakeRepository)
        {
            return await snakeRepository.GetAllAsync();
        }
        public async Task<Snake?> GetSnakeById(int id, [Service] ISnakeRepository snakeRepository)
        {
            return await snakeRepository.GetByIdAsync(id);
        }
    }
}
