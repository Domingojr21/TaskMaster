
namespace TaskManager.Infrastructure.Persistence.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task DeleteAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllWithInclude(List<string> properties);
        Task<T> GetByIdAsync(int id);
        Task UpdateAsync(int id, T entity);
    }
}