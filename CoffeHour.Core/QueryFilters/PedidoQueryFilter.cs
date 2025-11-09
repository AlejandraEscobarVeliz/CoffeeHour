// CoffeHour.Core/QueryFilters/PedidoQueryFilter.cs
namespace CoffeHour.Core.QueryFilters
{
    /// <summary>
    /// Filtros para consultas de pedidos con paginación.
    /// </summary>
    public class PedidoQueryFilter : PaginationQueryFilter
    {
        /// <summary>
        /// Filtrar por cliente específico
        /// </summary>
        public int? IdCliente { get; set; }

        /// <summary>
        /// Fecha inicial del rango de búsqueda
        /// </summary>
        public DateTime? FechaInicio { get; set; }

        /// <summary>
        /// Fecha final del rango de búsqueda
        /// </summary>
        public DateTime? FechaFin { get; set; }

        /// <summary>
        /// Estado del pedido: Pendiente, Preparando, Entregado, Cancelado
        /// </summary>
        public string? Estado { get; set; }

        /// <summary>
        /// Monto mínimo del pedido
        /// </summary>
        public decimal? MontoMin { get; set; }

        /// <summary>
        /// Monto máximo del pedido
        /// </summary>
        public decimal? MontoMax { get; set; }

        /// <summary>
        /// Ordenar por: fecha, total
        /// </summary>
        public string? OrderBy { get; set; }

        /// <summary>
        /// Orden descendente (true) o ascendente (false)
        /// </summary>
        public bool OrderDesc { get; set; } = true; // Por defecto: más recientes primero
    }
}
