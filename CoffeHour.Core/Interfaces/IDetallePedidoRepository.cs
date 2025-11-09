using CoffeHour.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeHour.Core.Interfaces
{
    public interface IDetallePedidoRepository : IBaseRepository<DetallesPedido> 
    {
        /* Task<IEnumerable<DetallesPedido>> GetAllAsync();
         Task<DetallesPedido?> GetByIdAsync(int id);
         Task AddAsync(DetallesPedido detalle);
         Task UpdateAsync(DetallesPedido detalle);
         Task DeleteAsync(int id);*/
        Task<IEnumerable<DetallesPedido>> GetByPedidoIdAsync(int pedidoId);
    }
}
