
using System.Linq.Expressions;

namespace CoffeHour.Core.Interfaces
{
    /// <summary>
    /// Interfaz genérica base para operaciones CRUD básicas.
    /// </summary>
    public interface IBaseRepository<T> where T : class
    {
        // ✅ Sin Task - método síncrono
        IEnumerable<T> GetAll();

        // ✅ Con Task - método asíncrono
        Task<T?> GetByIdAsync(int id);

        // ✅ Con Task - método asíncrono
        Task AddAsync(T entity);

        // ✅ Sin Task - método síncrono (solo marca como modificado)
        void Update(T entity);

        // ✅ Con Task - método asíncrono
        Task DeleteAsync(int id);

        // ✅ Para filtros avanzados
        IQueryable<T> Query(Expression<Func<T, bool>>? predicate = null);
    }
}