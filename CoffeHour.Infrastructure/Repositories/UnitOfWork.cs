using CoffeHour.Core.Entities;
using CoffeHour.Core.Interfaces;
using CoffeHour.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace CoffeHour.Infrastructure.Repositories
{
    /// <summary>
    /// Implementa el patrón Unit of Work para coordinar repositorios y transacciones.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoffeeHourContext _context;
        private IDbContextTransaction? _transaction;
        public IClienteRepository Clientes { get; }
        public IProductoRepository Productos { get; }
        public IPedidoRepository Pedidos { get; }
        public IDetallePedidoRepository Detalles { get; }


        public UnitOfWork(
            CoffeeHourContext context,
            IClienteRepository clientes,
            IProductoRepository productos,
            IPedidoRepository pedidos,
            IDetallePedidoRepository detalles)
        {
            _context = context;
            Clientes = clientes;
            Productos = productos;
            Pedidos = pedidos;
            Detalles = detalles;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
                await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        Task IUnitOfWork.SaveChangesAsync()
        {
            return SaveChangesAsync();
        }
    }
}
