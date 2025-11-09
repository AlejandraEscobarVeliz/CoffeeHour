using CoffeHour.Core.Interfaces;
using CoffeHour.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoffeHour.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación genérica del patrón Repositorio.
    /// </summary>
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly CoffeeHourContext _context;
        protected readonly DbSet<T> _entities;

        public BaseRepository(CoffeeHourContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() 
            => await _entities.ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _entities.FindAsync(id);

        public async Task AddAsync(T entity) => await _entities.AddAsync(entity);

        public async Task UpdateAsync(T entity) => _entities.Update(entity);

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
                _entities.Remove(entity);
        }

        public IQueryable<T> Query(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate != null ? _entities.Where(predicate) : _entities;
        }
    }

}
