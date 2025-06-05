using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Repositories.Generic;
using Core.Services.Generic;

namespace Application.Services.Generic
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        // Query
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> Delete(int id)
        {
            var obj = await _repository.GetByIdAsync(id);
            if (obj == null)
            {
                throw new Exception($"id {id} not found.");
            }                
            var deleted = await _repository.Delete(id);
            if (deleted)
                await _repository.SaveAsync();

            return deleted;
        }


        // Save
        public async Task SaveAsync()
        {
            await _repository.SaveAsync();
        }

    }
}
