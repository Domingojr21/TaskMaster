

using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure.Persistence.Context;

namespace TaskManager.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationContext _context;
        public GenericRepository(ApplicationContext context)
        {
            _context = context;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            _context.SaveChanges();
            return entity;
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();

        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            if (await _context.Set<T>().FindAsync(id) != null)
            {
                return await _context.Set<T>().FindAsync(id);
            }

            return null;
        }

        public virtual async Task<List<T>> GetAllWithInclude(List<string> properties)
        {
            var query = _context.Set<T>().AsQueryable();
            foreach (var property in properties)
            {
                query = query.Include(property);
            }

            return await query.ToListAsync();
        }

        public virtual async Task UpdateAsync(int id,T entity)
        {
            var entry = await _context.Set<T>().FindAsync(id);
            _context.Entry(entry).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            _context.Remove(entity);
            _context.SaveChanges();
        }
    }
}
