using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeHour.Core.Entities;
using CoffeHour.Core.DTOs;

namespace CoffeHour.Core.Interfaces
{
    public interface IPedidoRepository: IBaseRepository<Pedidos>

    {
        Task<Pedidos?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Pedidos>> GetDailyOrdersAsync(DateTime fecha);
        Task<int> CreateOrderWithDetailsAsync(Pedidos pedido, IEnumerable<DetallesPedido> detalles);


        Task<IEnumerable<Pedidos>> GetAllAsync();
        Task<Pedidos?> GetByIdAsync(int id);
        Task AddAsync(Pedidos pedido);
        Task UpdateAsync(Pedidos pedido);
        Task DeleteAsync(int id);

        // Casos de uso
        Task<int> CreateOrderAsync(Pedidos pedido, IEnumerable<DetallesPedido> detalles);
        Task<bool> ChangeOrderStatusAsync(int idPedido, string nuevoEstado);
        Task<SalesReportDTO> GetDailySalesReportAsync(DateTime fecha);
        //Task AddAsync(Pedidos dto);
    }
}

