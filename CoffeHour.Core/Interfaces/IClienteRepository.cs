using CoffeHour.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeHour.Core.Interfaces
{
    public interface IClienteRepository : IBaseRepository<Clientes>
    {
        Task<IEnumerable<Clientes>> GetAllAsync();
        Task<Clientes?> GetByIdAsync(int id);
        Task AddAsync(Clientes cliente);
        Task UpdateAsync(Clientes cliente);
        Task DeleteAsync(int id);
    }
}
