using CoffeHour.Core.Entities;
using CoffeHour.Core.Interfaces;
using CoffeHour.Core.DTOs;
using CoffeHour.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeHour.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly CoffeeHourContext _ctx;
        public PedidoRepository(CoffeeHourContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<Pedidos>> GetAllAsync() =>
            await _ctx.Pedidos.Include(p => p.Cliente)
                               .Include(p => p.DetallesPedido).ThenInclude(d => d.Producto)
                               .ToListAsync();

        public async Task<Pedidos?> GetByIdAsync(int id) =>
            await _ctx.Pedidos.Include(p => p.Cliente)
                              .Include(p => p.DetallesPedido).ThenInclude(d => d.Producto)
                              .FirstOrDefaultAsync(p => p.IdPedido == id);

        public async Task AddAsync(Pedidos pedido)
        {
            _ctx.Pedidos.Add(pedido);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(Pedidos pedido)
        {
            _ctx.Pedidos.Update(pedido);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _ctx.Pedidos.FindAsync(id);
            if (entity != null)
            {
                _ctx.Pedidos.Remove(entity);
                await _ctx.SaveChangesAsync();
            }
        }

        public async Task<int> CreateOrderAsync(Pedidos pedido, IEnumerable<DetallesPedido> detalles)
        {
            // Validaciones (mínimas)
            var detallesList = detalles?.ToList() ?? new List<DetallesPedido>();
            if (!detallesList.Any()) throw new InvalidOperationException("El pedido debe tener al menos un detalle.");

            // Recalcular subtotales a partir de precio actual
            foreach (var det in detallesList)
            {
                var producto = await _ctx.Productos.FindAsync(det.IdProducto);
                if (producto == null) throw new InvalidOperationException($"Producto {det.IdProducto} no existe.");
                if (det.Cantidad <= 0) throw new InvalidOperationException("Cantidad debe ser > 0.");
                det.Subtotal = producto.Precio * det.Cantidad;
            }

            pedido.Total = detallesList.Sum(d => d.Subtotal);
            pedido.Estado = "Pendiente";
            pedido.Fecha = DateTime.Now;
            pedido.DetallesPedido = detallesList;

            // Guardar en transacción
            using var tran = await _ctx.Database.BeginTransactionAsync();
            try
            {
                _ctx.Pedidos.Add(pedido);
                await _ctx.SaveChangesAsync();
                await tran.CommitAsync();
                return pedido.IdPedido;
            }
            catch
            {
                await tran.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> ChangeOrderStatusAsync(int idPedido, string nuevoEstado)
        {
            var pedido = await _ctx.Pedidos.FindAsync(idPedido);
            if (pedido == null) return false;

            var transiciones = new Dictionary<string, string[]>
            {
                { "Pendiente", new[] { "Preparando", "Cancelado" } },
                { "Preparando", new[] { "Entregado", "Cancelado" } },
                { "Entregado", new string[] { } }
            };

            var actual = pedido.Estado ?? "Pendiente";
            if (actual == nuevoEstado) return true;
            if (!transiciones.ContainsKey(actual) || !transiciones[actual].Contains(nuevoEstado))
                throw new InvalidOperationException($"No se puede cambiar de {actual} a {nuevoEstado}.");

            pedido.Estado = nuevoEstado;
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<SalesReportDTO> GetDailySalesReportAsync(DateTime fecha)
        {
            var inicio = fecha.Date;
            var fin = inicio.AddDays(1);

            var pedidos = await _ctx.Pedidos
                .Where(p => p.Fecha >= inicio && p.Fecha < fin && p.Estado == "Entregado")
                .ToListAsync();

            return new SalesReportDTO
            {
                Fecha = inicio,
                OrdersCount = pedidos.Count,
                TotalSales = pedidos.Sum(p => p.Total)
            };
        }
    }
}
