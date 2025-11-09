namespace CoffeHour.Core.QueryFilters
{
    /// <summary>
    /// Filtros para consultas de productos con paginación.
    /// </summary>
    public class ProductoQueryFilter : PaginationQueryFilter
    {
        /// <summary>
        /// Filtrar por categoría del producto
        /// </summary>
        public string? Categoria { get; set; }

        /// <summary>
        /// Filtrar por estado (Activo/Inactivo)
        /// </summary>
        public string? Estado { get; set; }

        /// <summary>
        /// Búsqueda por nombre del producto
        /// </summary>
        public string? Search { get; set; }

        /// <summary>
        /// Precio mínimo
        /// </summary>
        public decimal? PrecioMin { get; set; }

        /// <summary>
        /// Precio máximo
        /// </summary>
        public decimal? PrecioMax { get; set; }

        /// <summary>
        /// Ordenar por: Nombre, Precio, FechaCreacion
        /// </summary>
        public string? OrderBy { get; set; }

        /// <summary>
        /// Orden descendente
        /// </summary>
        public bool OrderDesc { get; set; } = false;
    }
}