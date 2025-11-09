using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeHour.Core.CustomEntities
{
    /// <summary>
    /// Parámetros que admite el endpoint de consulta de productos.
    /// </summary>
    public class ProductQueryParameters
    {
        public string? Categoria { get; set; }
        public string? Estado { get; set; }
        public string? Search { get; set; } // buscar por nombre
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; } // opcional: "precio", "fecha"
        public bool Desc { get; set; } = false;
    }
}

