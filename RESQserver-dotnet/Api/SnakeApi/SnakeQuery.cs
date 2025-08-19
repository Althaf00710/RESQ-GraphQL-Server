using Core.DTO;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.SnakeApi
{
    [ExtendObjectType<Query>]
    public class SnakeQuery
    {
        public async Task<IEnumerable<Snake>> GetSnakes([Service] ISnakeService service)
        {
            return await service.GetAllAsync();
        }
        public async Task<Snake?> GetSnakeById(int id, [Service] ISnakeService service)
        {
            return await service.GetByIdAsync(id);
        }
        public async Task<SnakePredicted> SnakeClassifier(IFile file, [Service] ISnakeService service)
        {
            return await service.SnakePredictor(file);
        }
    }
}
