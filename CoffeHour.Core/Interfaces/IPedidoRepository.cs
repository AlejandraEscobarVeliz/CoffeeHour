
using CoffeHour.Core.Entities;
using CoffeHour.Core.DTOs;

namespace CoffeHour.Core.Interfaces
{
    /// <summary>
    /// Repositorio para operaciones específicas de Pedidos.
    /// </summary>
    public interface IPedidoRepository: IBaseRepository<Pedidos>

    {
        Task<Pedidos?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Pedidos>> GetDailyOrdersAsync(DateTime fecha);
        Task<int> CreateOrderAsync(Pedidos pedido, IEnumerable<DetallesPedido> detalles);
        Task<bool> ChangeOrderStatusAsync(int idPedido, string nuevoEstado);
        Task<SalesReportDTO> GetDailySalesReportAsync(DateTime fecha);
    }
}

