// CoffeHour.Infrastructure/Repositories/PedidoRepository.cs
using CoffeHour.Core.Entities;
using CoffeHour.Core.Interfaces;
using CoffeHour.Core.DTOs;
using CoffeHour.Core.Exceptions;
using CoffeHour.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeHour.Infrastructure.Repositories
{
    public class PedidoRepository : BaseRepository<Pedidos>, IPedidoRepository
    {
        public PedidoRepository(CoffeeHourContext context) : base(context) { }

        // ✅ Caso de Uso 1: Crear pedido con detalles
        public async Task<int> CreateOrderAsync(Pedidos pedido, IEnumerable<DetallesPedido> detalles)
        {
            var detallesList = detalles?.ToList() ?? new List<DetallesPedido>();

            if (!detallesList.Any())
                throw new BusinessException("El pedido debe tener al menos un detalle.", 400);

            // Validar y calcular subtotales
            foreach (var det in detallesList)
            {
                var producto = await _context.Productos.FindAsync(det.IdProducto);
                if (producto == null)
                    throw new BusinessException($"Producto {det.IdProducto} no existe.", 404);

                if (det.Cantidad <= 0)
                    throw new BusinessException("Cantidad debe ser mayor a 0.", 400);

                det.Subtotal = producto.Precio * det.Cantidad;
            }

            pedido.Total = detallesList.Sum(d => d.Subtotal);
            pedido.Estado = "Pendiente";
            pedido.Fecha = DateTime.Now;
            pedido.DetallesPedido = detallesList;

            await AddAsync(pedido);
            

            return pedido.Id;
        }

        // ✅ Caso de Uso 2: Cambiar estado
        public async Task<bool> ChangeOrderStatusAsync(int idPedido, string nuevoEstado)
        {
            var pedido = await GetByIdAsync(idPedido);
            if (pedido == null) return false;

            var transiciones = new Dictionary<string, string[]>
            {
                { "Pendiente", new[] { "Preparando", "Cancelado" } },
                { "Preparando", new[] { "Entregado", "Cancelado" } },
                { "Entregado", new string[] { } },
                { "Cancelado", new string[] { } }
            };

            var actual = pedido.Estado ?? "Pendiente";
            if (actual == nuevoEstado) return true;

            if (!transiciones.ContainsKey(actual) || !transiciones[actual].Contains(nuevoEstado))
                throw new BusinessException($"No se puede cambiar de '{actual}' a '{nuevoEstado}'.", 400);

            pedido.Estado = nuevoEstado;
            Update(pedido);
            // ❌ NO guardar aquí

            return true;
        }

        // ✅ Caso de Uso 3: Reporte de ventas
        public async Task<SalesReportDTO> GetDailySalesReportAsync(DateTime fecha)
        {
            var inicio = fecha.Date;
            var fin = inicio.AddDays(1);

            var pedidos = await _context.Pedidos
                .Where(p => p.Fecha >= inicio && p.Fecha < fin && p.Estado == "Entregado")
                .ToListAsync();

            return new SalesReportDTO
            {
                Fecha = inicio,
                OrdersCount = pedidos.Count,
                TotalSales = pedidos.Sum(p => p.Total)
            };
        }

        // ✅ Métodos auxiliares
        public async Task<Pedidos?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.DetallesPedido)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Pedidos>> GetDailyOrdersAsync(DateTime fecha)
        {
            var inicio = fecha.Date;
            var fin = inicio.AddDays(1);

            return await _context.Pedidos
                .Where(p => p.Fecha >= inicio && p.Fecha < fin)
                .Include(p => p.Cliente)
                .Include(p => p.DetallesPedido)
                .ToListAsync();
        }

        public Task<int> CreateOrderWithDetailsAsync(Pedidos pedido, IEnumerable<DetallesPedido> detalles)
        {
            // Alias del método principal
            return CreateOrderAsync(pedido, detalles);
        }
    }
}