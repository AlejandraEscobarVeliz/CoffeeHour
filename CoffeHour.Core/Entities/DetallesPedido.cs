using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeHour.Core.Entities
{
    public partial class DetallesPedido: BaseEntity
    {
        [Key]
        [Column("IdDetalle")] 
        public new int Id { get; set; }

        [Required]
        [Column("IdPedido")]
        public int IdPedido { get; set; }

        [Required]
        [Column("IdProducto")]
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }

        public virtual Pedidos? Pedido { get; set; }
        public virtual Productos? Producto { get; set; }
    }
}
