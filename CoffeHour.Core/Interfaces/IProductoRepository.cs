using CoffeHour.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeHour.Core.Interfaces
{
    public interface IProductoRepository : IBaseRepository<Productos>
    {
        IEnumerable<object> GetAllQueryable();
        Task<IEnumerable<Productos>> GetFilteredAsync(string? categoria, string? estado);
    
    /* Task<IEnumerable<Productos>> GetAllAsync();
     Task<Productos?> GetByIdAsync(int id);
     Task AddAsync(Productos producto);
     Task UpdateAsync(Productos producto);
     Task DeleteAsync(int id);*/
}
}
