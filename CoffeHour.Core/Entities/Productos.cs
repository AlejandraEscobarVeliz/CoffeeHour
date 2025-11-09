using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeHour.Core.Entities
{
    public partial class Productos: BaseEntity
    {
        [Key]
        [Column("IdProducto")]
        public new int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Categoria { get; set; }
        public decimal Precio { get; set; }
        public string Estado { get; set; } = "Activo";
        public DateTime FechaCreacion { get; set; }

        public virtual ICollection<DetallesPedido>? DetallesPedido { get; set; }
    }
}
