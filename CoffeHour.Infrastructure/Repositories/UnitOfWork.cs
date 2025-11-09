// CoffeHour.Infrastructure/Repositories/UnitOfWork.cs
using CoffeHour.Core.Interfaces;
using CoffeHour.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace CoffeHour.Infrastructure.Repositories
{
    /// <summary>
    /// Implementa el patrón Unit of Work para coordinar repositorios y transacciones.
    /// Responsable de SaveChanges y manejo de transacciones.
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

        // ✅ Guardar cambios - responsabilidad del UnitOfWork
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // ✅ Manejo de transacciones
        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
                _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
