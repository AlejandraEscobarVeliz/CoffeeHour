using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoffeHour.Core.Interfaces
{
    /// <summary>
    /// Interfaz genérica base para operaciones CRUD básicas.
    /// </summary>
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        IQueryable<T> Query(Expression<Func<T, bool>>? predicate = null);
    }

}
