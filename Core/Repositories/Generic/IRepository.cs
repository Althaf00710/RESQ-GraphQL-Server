namespace Core.Repositories.Generic
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task Update(T entity);
        Task<bool> Delete(int id);
        Task SaveAsync();
    }
}
