// CoffeHour.Infrastructure/Repositories/BaseRepository.cs
using CoffeHour.Core.Interfaces;
using CoffeHour.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoffeHour.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación genérica del patrón Repositorio.
    /// No guarda cambios, solo registra operaciones en el contexto.
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

        // ✅ Método síncrono - devuelve IEnumerable
        public IEnumerable<T> GetAll()
            => _entities.AsEnumerable();

        // ✅ Método asíncrono
        public async Task<T?> GetByIdAsync(int id)
            => await _entities.FindAsync(id);

        // ✅ Método asíncrono
        public async Task AddAsync(T entity)
            => await _entities.AddAsync(entity);

        // ✅ Método síncrono - solo marca como modificado
        public void Update(T entity)
            => _entities.Update(entity);

        // ✅ Método asíncrono
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
                _entities.Remove(entity);
        }

        // ✅ Para filtros - devuelve IQueryable
        public IQueryable<T> Query(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate != null ? _entities.Where(predicate) : _entities;
        }
    }
}