using CoffeHour.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeHour.Core.Interfaces
{
    /// <summary>
    /// Define una unidad de trabajo para manejar transacciones y múltiples repositorios.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IClienteRepository Clientes { get; }
        IProductoRepository Productos { get; }
        IPedidoRepository Pedidos { get; }
        Task SaveChangesAsync();
        IDetallePedidoRepository Detalles { get; }
        //Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }

}
